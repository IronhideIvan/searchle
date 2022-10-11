using System;
using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Interfaces;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Business.Services
{
  public class LexicalSearchService : ILexicalSearchService
  {
    private ILexicalSearchDao _searchDao;

    public LexicalSearchService(ILexicalSearchDao searchDao)
    {
      _searchDao = searchDao;
    }

    public async Task<IEnumerable<LexicalWord>> SearchWordsAsync(LexicalSearch searchQuery, SelectorCollection? selectors = null)
    {
      return await _searchDao.SearchWordsAsync(searchQuery, selectors);
    }
  }
}
