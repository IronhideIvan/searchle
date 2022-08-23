using System;
using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Wordnet.Data.Queries;

namespace Wordnet.Data.Dao
{
  public class WordnetLexicalSearchDao : ILexicalSearchDao
  {
    private IDictionaryDataProvider _dataProvider;

    public WordnetLexicalSearchDao(IDictionaryDataProvider dataProvider)
    {
      _dataProvider = dataProvider;
    }

    public async Task<IEnumerable<LexicalWord>> SearchWordsAsync(LexicalSearch search, SelectorCollection? selectors = null)
    {
      var query = new GetWordSearchQuery
      {
        SearchQuery = search,
        Selectors = selectors
      };

      var result = await _dataProvider.QueryAsync(query);
      return result;
    }
  }
}
