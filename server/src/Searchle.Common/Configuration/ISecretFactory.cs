using System;

namespace Searchle.Common.Configuration
{
  public interface ISecretFactory
  {
    ISecret Create(string key);
  }
}
