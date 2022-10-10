using System;
using Searchle.Configuration.Interfaces;

namespace Searchle.Configuration.Implementations
{
  public class PlainAsDaySecretRepository : ISecretRepository
  {
    public PlainAsDaySecretRepository()
    {
    }

    public string FetchSecretValue(string valueKey)
    {
      return valueKey;
    }

    public Task<string> FetchSecretValueAsync(string valueKey)
    {
      return Task.FromResult(valueKey);
    }
  }
}
