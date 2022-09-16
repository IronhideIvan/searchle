namespace Searchle.GraphQL.Filters
{
  public class SearchleErrorFilter : IErrorFilter
  {
    public IError OnError(IError error)
    {
      if (error.Exception == null)
      {
        return error.RemoveExtensions();
      }

      return error.WithCode("ERR")
        .RemoveException()
        .RemoveExtensions();
    }
  }
}
