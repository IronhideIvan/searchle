using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Searchle.Common.Exceptions;
using Searchle.Common.Logging;
using Path = System.IO.Path;

namespace Searchle.GraphQL.ApplicationStartup
{
  public static class ConfigurationLoader
  {
    public static SearchleAppConfig LoadConfiguration(this IServiceCollection services, IAppLogger<Startup> logger)
    {
      string configurationPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
      logger.Debug("Searching for configuration at {ConfigurationLocation}", configurationPath);

      if (!File.Exists(configurationPath))
      {
        logger.Critical("Configuration NOT found at {ConfigurationLocation}", configurationPath);
        throw new SearchleCriticalException("Unable to locate application configuration");
      }

      logger.Information("Configuration found at {ConfigurationLocation}", configurationPath);
      var configFileContents = File.ReadAllText(configurationPath);
      var fullConfig = JsonConvert.DeserializeObject<JObject>(configFileContents);

      var appConfig = fullConfig?["AppConfig"]?.ToObject<SearchleAppConfig>();
      if (appConfig == null)
      {
        logger.Critical("Unable to read application configuration details within configuation file. File located in {ConfigurationLocation}", configurationPath);
        throw new SearchleCriticalException("Unable to read application configuration within configuration file.");
      }

      logger.Information("Successfully parsed configuration file");

      services.AddSingleton<SearchleAppConfig>(appConfig);
      if (appConfig.Logging != null)
      {
        services.AddSingleton<AppLoggingConfig>(appConfig.Logging);
      }
      else
      {
        var loggingConfig = new AppLoggingConfig
        {
          LogLevel = AppLogLevel.Error
        };
      }

      if (appConfig.DictionaryConnectionConfig == null)
      {
        logger.Critical("Unable to find dictionary connection details in configuation file. File located in {ConfigurationLocation}", configurationPath);
        throw new SearchleCriticalException("Unable to find dictionary connection details in configuration file.");
      }

      return appConfig;
    }
  }
}
