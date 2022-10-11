using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Common.Interfaces
{
  public interface ILexicalWordDao
  {
    Task<LexicalWord?> GetLexicalWordAsync(int id, SelectorCollection? selectors = null);
  }
}
