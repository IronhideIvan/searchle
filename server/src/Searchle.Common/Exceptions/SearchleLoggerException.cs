using System;

namespace Searchle.Common.Exceptions
{
  public class SearchleLoggerException : SearchleDomainException
  {
    public SearchleLoggerException(string message, Exception inner) : base(message, inner)
    {

    }
  }
}
