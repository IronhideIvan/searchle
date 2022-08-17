using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migrations.Core.Interfaces;
using Migrations.Core.Services;

namespace Migrations.Core
{
  public class Migrator
  {
    public async Task Run(params Assembly[] migrationAssemblies)
    {
      try
      {
        var startup = new Startup(migrationAssemblies);
        var host = startup.AppStartup();

        using (var scope = host.Services.CreateScope())
        {
          var databaseMigrator = scope.ServiceProvider.GetRequiredService<IDataaseMigrator>();
          var logger = scope.ServiceProvider.GetRequiredService<ILogger<Migrator>>();

          try
          {
            logger.LogInformation("Migration starting...");
            await databaseMigrator.EnsureDatabaseCreated();
            databaseMigrator.MigrateUp();
            logger.LogInformation("Migration complete!");
          }
          catch (Exception ex)
          {
            string message = $"ERROR: {ex.Message}\nSTACKTRACE:\n{ex.StackTrace}";
            logger.LogCritical(message);
          }
        }

      }
      catch (Exception ex)
      {
        string message = $"ERROR: {ex.Message}\nSTACKTRACE:\n{ex.StackTrace}";
        Console.WriteLine(message);
      }
    }
  }
}
