using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migrations.Core.Interfaces;

namespace Migrations.Core
{
  public class Migrator<T> where T : class
  {
    public async Task Run(params Assembly[] migrationAssemblies)
    {
      try
      {
        var startup = new Startup<T>(migrationAssemblies);
        var host = startup.AppStartup();

        using (var scope = host.Services.CreateScope())
        {
          var databaseMigrator = scope.ServiceProvider.GetRequiredService<IDataaseMigrator>();
          var logger = scope.ServiceProvider.GetRequiredService<ILogger<Migrator<T>>>();

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
