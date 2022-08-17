using System.IO;
using System.Threading.Tasks;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;
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
            var script = await File.ReadAllTextAsync(scriptPath);
            await cnn.ExecuteAsync(script);
          }
        }
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