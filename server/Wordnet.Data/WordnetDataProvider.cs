using System;
using Searchle.DataAccess.Common.Interfaces;
using Searchle.Dictionary.Common.Interfaces;

namespace Wordnet.Data
{
  public class WordnetDataProvider : IDictionaryDataProvider
  {
    private IDataProvider _dataProvider { get; set; }

    public WordnetDataProvider(IDataProvider dataProvider)
    {
      _dataProvider = dataProvider;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(IQuery<T> query)
    {
      return await _dataProvider.QueryAsync(query);
    }
  }
}
