using System;

namespace Searchle.Common.Configuration
{
  public interface ISecretRepository
  {
    string FetchSecretValue(string valueKey);
    Task<string> FetchSecretValueAsync(string valueKey);
  }
}
