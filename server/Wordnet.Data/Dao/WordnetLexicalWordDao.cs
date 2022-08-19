using System;
using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Wordnet.Data.Queries;

namespace Wordnet.Data.Dao
{
  public class WordnetLexicalWordDao : ILexicalWordDao
  {
    private IDictionaryDataProvider _dataProvider;

    public WordnetLexicalWordDao(IDictionaryDataProvider dataProvider)
    {
      _dataProvider = dataProvider;
    }

    public async Task<LexicalWord?> GetLexicalWordAsync(int id, SelectorCollection? selectors = null)
    {
      var query = new GetWordQuery
      {
        WordId = id,
        Selectors = selectors
      };

      var result = await _dataProvider.QueryAsync(query);
      return result.FirstOrDefault();
    }
  }
}
