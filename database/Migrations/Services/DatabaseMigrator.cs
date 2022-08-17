using System.Threading.Tasks;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.Logging;
using Migrations.Models;

namespace Migrations.Services
{
    internal sealed class DatabaseMigrator : IDataaseMigrator
    {
        private readonly ILogger<DatabaseMigrator> _logger;
        private readonly IDataService _dataService;
        private readonly IScriptService _scriptService;
        private readonly IMigrationRunner _migrationRunner;
        private readonly AppConfig _config;


        public DatabaseMigrator(
            AppConfig config,
            IDataService dataService,
            IScriptService scriptService,
            IMigrationRunner migrationRunner,
            ILogger<DatabaseMigrator> logger
            )
        {
            _config = config;
            _scriptService = scriptService;
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
                    Catalog = "portfolio",
                    Schema = "account"
                });

                if (ret == null)
                {
                    var script = await _scriptService.GetScriptAsync(ScriptKeys.CreateDatabase);
                    await cnn.ExecuteAsync(script);

                    script = await _scriptService.GetScriptAsync(ScriptKeys.SeedData);
                    await cnn.ExecuteAsync(script);
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