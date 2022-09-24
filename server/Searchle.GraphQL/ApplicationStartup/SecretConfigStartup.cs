using Newtonsoft.Json;
using Searchle.Common.Configuration;
using Searchle.Common.Logging;
using Searchle.Dictionary.Business.Configuration;

namespace Searchle.GraphQL.ApplicationStartup
{
  public static class SecretConfigStartup
  {
    public static IServiceCollection AddApplicationSecrets(this IServiceCollection services, IAppLoggerFactory loggerFactory, IWebHostEnvironment env, IConfiguration rootConfig)
    {
      var rootConfigSection = rootConfig.GetSection("Searchle").AsEnumerable();
      services.AddSingleton<ISecretRepository, LocalEnvironmentSecretRepository>(f =>
      {
        return new LocalEnvironmentSecretRepository(rootConfigSection, loggerFactory);
      });
      services.AddSingleton<ISecretFactory, PlaintextSecretFactory>();
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
