using System;
using Searchle.Configuration.Interfaces;

namespace Searchle.Configuration.Implementations
{
  public class LocalEnvironmentSecretRepository : ISecretRepository
  {
    private Dictionary<string, string> _configValues = new Dictionary<string, string>();

    public LocalEnvironmentSecretRepository(
      IEnumerable<KeyValuePair<string, string>> configValues)
    {
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
