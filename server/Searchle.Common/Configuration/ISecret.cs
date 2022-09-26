using System;

namespace Searchle.Common.Configuration
{
  public interface ISecret
  {
    string Key { get; }
    string GetValue();
    Task<string> GetValueAsync();
  }
}
