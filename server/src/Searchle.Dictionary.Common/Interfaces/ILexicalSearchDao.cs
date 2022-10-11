using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Common.Interfaces
{
  public interface ILexicalSearchDao
  {
    Task<IEnumerable<LexicalWord>> SearchWordsAsync(LexicalSearch search, SelectorCollection? selectors = null);
  }
}
