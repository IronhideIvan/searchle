using System;
using Searchle.DataAccess.Common;
using Searchle.Dictionary.Common.Models;

namespace Searchle.Dictionary.Business.Services
{
  public interface ILexicalSearchService
  {
    Task<IEnumerable<LexicalWord>> SearchWordsAsync(LexicalSearch searchQuery, SelectorCollection? selectors = null);
  }
}
