using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Searchle.Common.Exceptions;
using Searchle.Common.Logging;
using Searchle.Configuration.Converters;
using Searchle.Configuration.Implementations;
using Searchle.Configuration.Interfaces;
using Searchle.Configuration.Models;

namespace Searchle.GraphQL.ApplicationStartup
{
  public static class SecretConfigStartup
  {
    public static SecretConfiguration RegisterSecretConfig(this IServiceCollection services, IAppLoggerFactory loggerFactory, JObject baseConfig)
    {
      var logger = loggerFactory.Create<Startup>();

      var secretConfig = baseConfig?["AppConfig"]?["Secrets"]?.ToObject<SecretConfiguration>();
      if (secretConfig == null)
      {
        logger.Critical("Unable to read secret configuration details within configuation file.");
        throw new SearchleCriticalException("Unable to read secret configuration within configuration file.");
      }

      services.AddSingleton<SecretConfiguration>(secretConfig);
      return secretConfig;
    }

    public static IServiceCollection RegisterSecretsServices(
      this IServiceCollection services,
      IAppLoggerFactory loggerFactory,
      IWebHostEnvironment env,
      IConfiguration rootConfig,
      SecretConfiguration secretConfig
    )
    {
      var logger = loggerFactory.Create<Startup>();
      var rootConfigSection = rootConfig.GetSection("Searchle").AsEnumerable();

      // Initialize the secret source
      if (secretConfig.Source == SecretSource.Environment)
      {
        services.AddSingleton<ISecretRepository, LocalEnvironmentSecretRepository>(f =>
        {
          return new LocalEnvironmentSecretRepository(rootConfigSection);
        });
      }
      // Should never really be used, but useful for development and testing purposes
      else if (secretConfig.Source == SecretSource.None)
      {
        services.AddSingleton<ISecretRepository, PlainAsDaySecretRepository>();
      }
      else
      {
        throw new NotImplementedException();
      }

      // Initialize the secret factories
      if (secretConfig.Encrypted)
      {
        services.AddSingleton<ISecretFactory, EncryptedSecretFactory>();
      }
      else if (env.IsDevelopment())
      {
        services.AddSingleton<ISecretFactory, PlaintextSecretFactory>();
      }
      else
      {
        logger.Critical("Invalid Encryption option for application secrets. Secrets can only be set to 'Not Encrypted' if the application is running on a development environment.");
        throw new SearchleCriticalException("Invalid Encryption option for application secrets. Secrets can only be set to 'Not Encrypted' if the application is running on a development environment.");
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

      return services;
    }
  }
}
