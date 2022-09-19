namespace Searchle.Common.Exceptions
{
  public class SearchleDomainException : Exception
  {
    public SearchleDomainException(string message) : base(message)
    {

    }

    public SearchleDomainException(string message, Exception inner) : base(message, inner)
    {

    }
  }
}
