using FluentMigrator.Runner;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Migrations.Core.Models;
using Migrations.Core.Interfaces;
using Migrations.Core.Services;

namespace Migrations.Core
{
  internal sealed class Startup
  {
    public void BuildConfig(IConfigurationBuilder builder)
    {
      builder.SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
          .AddEnvironmentVariables();
    }

    public IHost AppStartup()
    {
      var builder = new ConfigurationBuilder();
      BuildConfig(builder);

      var config = builder.Build();

      var host = Host.CreateDefaultBuilder()
                  .ConfigureServices(ConfigureServices)
                  .Build();

      return host;
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
      // Config
      var jsonConfig = File.ReadAllText("./appsettings.json");
      var config = JsonConvert.DeserializeObject<AppSettings>(jsonConfig);
      services.AddSingleton(config.AppConfig);

      // Services
      services.AddScoped<IDataService, DataService>();
      services.AddScoped<IDataaseMigrator, DatabaseMigrator>();

      // Fluent Migrator
      var provider = services.BuildServiceProvider();
      var dataService = provider.GetService<IDataService>();

      services.AddFluentMigratorCore();
      services.ConfigureRunner(rb => rb
              .AddPostgres()
              .WithGlobalConnectionString(dataService.GetConnectionString())
              .ScanIn(typeof(Startup).Assembly).For.Migrations());
      services.AddLogging(lb => lb.AddFluentMigratorConsole());
    }
  }
}