using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migrations.Services;

namespace Migrations
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var startup = new Startup();
                var host = startup.AppStartup();

                using (var scope = host.Services.CreateScope())
                {
                    var databaseMigrator = scope.ServiceProvider.GetRequiredService<IDataaseMigrator>();
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

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
