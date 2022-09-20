using System;

namespace Searchle.Common.Logging
{
  public interface IAppLoggerFactory
  {
    IAppLogger<T> Create<T>();
  }
}
