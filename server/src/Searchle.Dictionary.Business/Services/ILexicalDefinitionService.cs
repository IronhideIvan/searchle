using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Business.Services
{
  public interface ILexicalDefinitionService
  {
    Task<IEnumerable<LexicalDefinition>> GetLexicalDefinitionsByWord(int wordid, SelectorCollection? selectors = null);
    Task<LexicalDefinition?> GetLexicalDefinition(int definitionId, SelectorCollection? selectors = null);
  }
}
