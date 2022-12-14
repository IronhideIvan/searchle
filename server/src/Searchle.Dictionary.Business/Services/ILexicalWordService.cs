using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Business.Services
{
  public interface ILexicalWordService
  {
    Task<LexicalWord?> GetWordAsync(int id, SelectorCollection? selectors = null);
  }
}
