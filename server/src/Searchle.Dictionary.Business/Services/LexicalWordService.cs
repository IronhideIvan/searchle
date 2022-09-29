using System;
using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Interfaces;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Business.Services
{
  public class LexicalWordService : ILexicalWordService
  {
    private ILexicalWordDao _wordDao;

    public LexicalWordService(ILexicalWordDao wordDao)
    {
      _wordDao = wordDao;
    }

    public async Task<LexicalWord?> GetWordAsync(int id, SelectorCollection? selectors = null)
    {
      return await _wordDao.GetLexicalWordAsync(id, selectors);
    }
  }
}
