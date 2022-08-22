using System;

namespace Searchle.Dictionary.Common.Models
{
  public class LexicalSearch
  {
    public IEnumerable<string>? SearchTerms { get; set; }
    public int LetterCount { get; set; }
    public IEnumerable<char>? MustInclude { get; set; }
    public IEnumerable<char>? MustExclude { get; set; }
  }
}
