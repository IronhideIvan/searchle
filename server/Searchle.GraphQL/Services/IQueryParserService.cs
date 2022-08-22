using Searchle.Dictionary.Common.Models;

namespace Searchle.GraphQL.Services
{
  public interface IQueryParserService
  {
    LexicalSearch ParseQueryString(string searchQuery);
  }
}
