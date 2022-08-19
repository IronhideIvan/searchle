using System;
using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Interfaces;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Data.Services
{
  public class LexicalDefinitionService : ILexicalDefinitionService
  {
    private ILexicalDefinitionDao _definitionDao;

    public LexicalDefinitionService(ILexicalDefinitionDao definitionDao)
    {
      _definitionDao = definitionDao;
    }

    public async Task<IEnumerable<LexicalDefinition>> GetLexicalDefinitionsByWord(int wordid, SelectorCollection? selectors = null)
    {
      return await _definitionDao.GetLexicalDefintionsByWordAsync(wordid, selectors);
    }

    public async Task<LexicalDefinition?> GetLexicalDefinition(int definitionId, SelectorCollection? selectors = null)
    {
      return await _definitionDao.GetLexicalDefintionAsync(definitionId, selectors);
    }
  }
}
