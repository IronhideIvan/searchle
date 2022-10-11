using FluentMigrator.Runner;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Migrations.Core.Models;
using Migrations.Core.Interfaces;
using Migrations.Core.Services;
using System.Reflection;
using Searchle.Configuration.Models;
using Searchle.Configuration.Interfaces;
using Searchle.Configuration.Implementations;
using Searchle.Configuration.Converters;

namespace Migrations.Core
{
  internal sealed class Startup<T> where T : class
  {
    private IEnumerable<Assembly> _migrationAssemblies;
    private IConfiguration _configuration;

    public Startup(IEnumerable<Assembly> migrationAssemblies)
    {
      _migrationAssemblies = migrationAssemblies;
    }

    public void BuildConfig(IConfigurationBuilder builder)
    {
      builder.SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
          // .AddUserSecrets<T>()
          .AddEnvironmentVariables();
    }

    public IHost AppStartup()
    {
      var builder = new ConfigurationBuilder();
      BuildConfig(builder);

      _configuration = builder.Build();

      var host = Host.CreateDefaultBuilder()
                  .ConfigureServices(ConfigureServices)
                  .Build();

      return host;
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
      // Secrets
      var secretJson = File.ReadAllText("./secretsettings.json");
      var secretConfig = JsonConvert.DeserializeObject<SecretConfiguration>(secretJson);

      var rootConfigSection = _configuration.GetSection("Wordnet").AsEnumerable();

      // Initialize the secret source
      if (secretConfig.Source == SecretSource.None)
      {
        services.AddSingleton<ISecretRepository, PlainAsDaySecretRepository>();
      }
      else if (secretConfig.Source == SecretSource.Environment)
      {
        services.AddSingleton<ISecretRepository, LocalEnvironmentSecretRepository>(f =>
        {
          return new LocalEnvironmentSecretRepository(rootConfigSection);
        });
      }
      else
      {
        throw new NotImplementedException("ISecretRepository");
      }

      // Initialize the secret factories
      if (secretConfig.Encrypted)
      {
        services.AddSingleton<ISecretFactory, EncryptedSecretFactory>();
      }
      else
      {
        services.AddSingleton<ISecretFactory, PlaintextSecretFactory>();
      }

      // Initialize the JSON converter, so that we can accurately parse secret values
      services.AddTransient<Newtonsoft.Json.JsonConverter<ISecret>, SecretJsonConverter>();
      var serviceProvider = services.BuildServiceProvider();

      JsonConvert.DefaultSettings = () =>
      {
        return new JsonSerializerSettings
        {
          Converters = new List<JsonConverter> {
              serviceProvider.GetService<Newtonsoft.Json.JsonConverter<ISecret>>()!
          }
        };
      };


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
      var cnnSecret = dataService.GetConnectionStringAsync();
      if (!cnnSecret.IsCompleted)
      {
        cnnSecret.Wait();
      }

      var cnnString = cnnSecret.Result;

      services.AddFluentMigratorCore();
      services.ConfigureRunner(rb =>
      {
        rb.AddPostgres()
              .WithGlobalConnectionString(cnnString);

        foreach (var ass in _migrationAssemblies)
        {
          rb.ScanIn(ass).For.Migrations();
        }
      });

      services.AddLogging(lb => lb.AddFluentMigratorConsole());
    }
  }
}