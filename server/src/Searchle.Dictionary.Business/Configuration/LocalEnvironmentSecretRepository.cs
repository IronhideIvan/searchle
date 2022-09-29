using System;
using Searchle.Common.Configuration;
using Searchle.Common.Logging;

namespace Searchle.Dictionary.Business.Configuration
{
  public class LocalEnvironmentSecretRepository : ISecretRepository
  {
    private Dictionary<string, string> _configValues = new Dictionary<string, string>();

    public LocalEnvironmentSecretRepository(
      IEnumerable<KeyValuePair<string, string>> configValues,
      IAppLoggerFactory loggerFactory)
    {
      var logger = loggerFactory.Create<LocalEnvironmentSecretRepository>();
      logger.Debug("Identified configuration keys: {ConfigKey}", configValues.ToList().Select(v => v.Key));
      foreach (var val in configValues)
      {
        _configValues.Add(val.Key, val.Value);
      }
    }

    public string FetchSecretValue(string valueKey)
    {
      var value = _configValues.ContainsKey(valueKey)
        ? _configValues[valueKey]
        : string.Empty;

      return value;
    }

    public Task<string> FetchSecretValueAsync(string valueKey)
    {
      return Task.FromResult(FetchSecretValue(valueKey));
    }
  }
}
