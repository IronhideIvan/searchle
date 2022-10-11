using System;
using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Wordnet.Data.Queries;

namespace Wordnet.Data.Dao
{
  public class WordnetLexicalDefinitionDao : ILexicalDefinitionDao
  {
    private IDictionaryDataProvider _dataProvider;

    public WordnetLexicalDefinitionDao(IDictionaryDataProvider dataProvider)
    {
      _dataProvider = dataProvider;
    }

    public async Task<IEnumerable<LexicalDefinition>> GetLexicalDefintionsByWordAsync(int wordId, SelectorCollection? selectors = null)
    {
      var query = new GetWordDefinitionsByWordIdQuery
      {
        WordId = wordId,
        Selectors = selectors
      };

      var result = await _dataProvider.QueryAsync(query);
      return result;
    }

    public async Task<LexicalDefinition?> GetLexicalDefintionAsync(int definitionId, SelectorCollection? selectors = null)
    {
      var query = new GetWordDefinition
      {
        DefinitionId = definitionId,
        Selectors = selectors
      };

      var result = await _dataProvider.QueryAsync(query);
      return result.FirstOrDefault();
    }
  }
}
