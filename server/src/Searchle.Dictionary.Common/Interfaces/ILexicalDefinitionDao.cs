using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Common.Interfaces
{
  public interface ILexicalDefinitionDao
  {
    Task<IEnumerable<LexicalDefinition>> GetLexicalDefintionsByWordAsync(int wordId, SelectorCollection? selectors = null);
    Task<LexicalDefinition?> GetLexicalDefintionAsync(int definitionId, SelectorCollection? selectors = null);
  }
}
