using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Searchle.Common.Exceptions;
using Searchle.Common.Logging;
using Path = System.IO.Path;

namespace Searchle.GraphQL.ApplicationStartup
{
  public static class AppConfigurationStartup
  {
    public static JObject LoadConfiguration(this IServiceCollection services, IAppLoggerFactory loggerFactory, IWebHostEnvironment env)
    {
      var logger = loggerFactory.Create<Startup>();

      string configurationPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
      logger.Debug("Searching for configuration at {ConfigurationLocation}", configurationPath);

      if (!File.Exists(configurationPath))
      {
        logger.Critical("Configuration NOT found at {ConfigurationLocation}", configurationPath);
        throw new SearchleCriticalException("Unable to locate application configuration");
      }

      logger.Debug("Configuration found at {ConfigurationLocation}", configurationPath);
      var configFileContents = File.ReadAllText(configurationPath);
      var baseConfig = JsonConvert.DeserializeObject<JObject>(configFileContents);
      if (baseConfig == null)
      {
        logger.Critical("Unable to parse configuation file. File located in {ConfigurationLocation}", configurationPath);
        throw new SearchleCriticalException("Unable to parse configuration file.");
      }

      // look for environment configuration
      string environmentConfigPath = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.{env.EnvironmentName}.json");
      if (File.Exists(environmentConfigPath))
      {
        var envConfigContents = File.ReadAllText(environmentConfigPath);
        var envConfig = JsonConvert.DeserializeObject<JObject>(envConfigContents);
        if (envConfig != null)
        {
          baseConfig.Merge(envConfig,
            new JsonMergeSettings
            {
              MergeArrayHandling = MergeArrayHandling.Union
            });
          logger.Debug("Loaded envionment configuration file from path {EnvironmentConfigurationLocation}", environmentConfigPath);
        }
        else
        {
          logger.Error("Unable to parse environment configuation file. File located in {EnvironmentConfigurationLocation}", environmentConfigPath);
        }
      }
      else
      {
        logger.Debug("No environment specific configuration detected. Path searched: {EnvironmentConfigurationLocation}", environmentConfigPath);
      }

      return baseConfig;
    }

    public static SearchleAppConfig RegisterAppConfig(this IServiceCollection services, IAppLoggerFactory loggerFactory, JObject baseConfig)
    {
      var logger = loggerFactory.Create<Startup>();

      var appConfig = baseConfig?["AppConfig"]?.ToObject<SearchleAppConfig>();
      if (appConfig == null)
      {
        logger.Critical("Unable to read application configuration details within configuation file.");
        throw new SearchleCriticalException("Unable to read application configuration within configuration file.");
      }

      logger.Information("Successfully parsed configuration file");

      services.AddSingleton<SearchleAppConfig>(appConfig);

      if (appConfig.DictionaryConnectionConfig == null)
      {
        logger.Critical("Unable to find dictionary connection details in configuation file.");
        throw new SearchleCriticalException("Unable to find dictionary connection details in configuration file.");
      }

      return appConfig;
    }

    public static IServiceCollection RegisterLoggingConfig(this IServiceCollection services, IAppLoggerFactory loggerFactory, SearchleAppConfig appConfig)
    {
      var logger = loggerFactory.Create<Startup>();

      if (appConfig.Logging == null)
      {
        logger.Warning("No logging configuration found. Falling back to default logging configuration");
        appConfig.Logging = new AppLoggingConfig
        {
          LogLevel = AppLogLevel.Error
        };
      }
      services.AddSingleton<AppLoggingConfig>(appConfig.Logging);

      return services;
    }
  }
}
