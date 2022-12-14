using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;
using Migrations.Core.Errors;
using Migrations.Core.Interfaces;
using Migrations.Core.Models;

namespace Migrations.Core.Services
{
  internal sealed class DatabaseMigrator : IDataaseMigrator
  {
    private readonly ILogger<DatabaseMigrator> _logger;
    private readonly IDataService _dataService;
    private readonly IMigrationRunner _migrationRunner;
    private readonly AppConfig _config;


    public DatabaseMigrator(
        AppConfig config,
        IDataService dataService,
        IMigrationRunner migrationRunner,
        ILogger<DatabaseMigrator> logger
        )
    {
      _config = config;
      _dataService = dataService;
      _migrationRunner = migrationRunner;
      _logger = logger;
    }

    public async Task EnsureDatabaseCreated()
    {
      _logger.LogInformation("Verifying database exists...");
      using (var cnn = await _dataService.ConnectAsync())
      {
        var ret = await cnn.ExecuteScalarAsync(@"
                    select 1 
                    from information_schema.tables 
                    where table_catalog = :Catalog
                    and table_schema = :Schema
                    limit 1;", new
        {
          Catalog = _config.Metadata.Catalog,
          Schema = _config.Metadata.Schema
        });

        if (ret == null)
        {
          foreach (var scriptPath in _config.SetupScripts)
          {
            string tempPath = scriptPath;
            bool isGz = false;
            if (Path.GetExtension(scriptPath).ToLowerInvariant().Equals(".gz"))
            {
              tempPath = await Decompress(scriptPath);
              isGz = true;
            }

            try
            {
              // For massive files we need to read them from a stream in order to
              // prevent any memory exceptions
              using (var fs = new FileStream(tempPath, FileMode.Open))
              {
                using (var sr = new StreamReader(fs))
                {
                  var block = new StringBuilder();
                  string line = await sr.ReadLineAsync();
                  int statementCount = 0;
                  while (line != null)
                  {
                    block.AppendLine(line);
                    if (line.EndsWith(";"))
                    {
                      statementCount += 1;
                    }
                    if (statementCount > 1000)
                    {
                      await cnn.ExecuteAsync(block.ToString());
                      block.Clear();
                      statementCount = 0;
                    }
                    line = await sr.ReadLineAsync();
                  }
                  if (statementCount > 0)
                  {
                    await cnn.ExecuteAsync(block.ToString());
                  }
                }
              }
            }
            catch (Exception ex)
            {
              throw new MigrationException($"Error executing script '{scriptPath}'.\nError: {ex.Message}", ex);
            }
            finally
            {
              if (isGz)
              {
                File.Delete(tempPath);
              }
            }
          }
        }
      }
    }

    private async Task<string> Decompress(string filepath)
    {
      using (FileStream originalFileStream = File.OpenRead(filepath))
      {
        string currentFileName = filepath;
        string newFileName = currentFileName.Remove(currentFileName.Length - Path.GetExtension(filepath).Length);

        using (FileStream decompressedFileStream = File.Create(newFileName))
        {
          using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
          {
            await decompressionStream.CopyToAsync(decompressedFileStream);
            _logger.LogInformation("Decompressed: {0}", filepath);
          }
        }

        return newFileName;
      }
    }

    public void MigrateUp(long? version = null)
    {
      if (version.HasValue)
      {
        _migrationRunner.MigrateUp(version.Value);
      }
      else
      {
        _migrationRunner.MigrateUp();
      }
    }

    public void MigrateDown(long version)
    {
      _migrationRunner.MigrateDown(version);
    }
  }
}