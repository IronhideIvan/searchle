using Searchle.Common.Logging;

namespace Searchle.GraphQL.Filters
{
  public class SearchleErrorFilter : IErrorFilter
  {
    private IAppLogger<SearchleErrorFilter> _logger;

    public SearchleErrorFilter(IAppLoggerFactory loggerFactory)
    {
      _logger = loggerFactory.Create<SearchleErrorFilter>();
    }

    public IError OnError(IError error)
    {
      if (error.Exception != null)
      {
        _logger.Error("Error encountered in GraphQL app: {GraphQLException}", error.Exception, error.Exception.Message);
      }
      else
      {
        _logger.Debug("GraphQL Error {GraphQLError}", new { Code = error.Code, Message = error.Message });
      }

      return error.WithCode("ERR")
        .RemoveException()
        .RemoveExtensions();
    }
  }
}
