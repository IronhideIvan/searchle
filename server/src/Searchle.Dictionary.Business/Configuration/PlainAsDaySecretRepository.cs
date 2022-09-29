using System;
using Searchle.Common.Configuration;
using Searchle.Common.Logging;

namespace Searchle.Dictionary.Business.Configuration
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
