using System;
using Searchle.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Searchle.GraphQL.Schema.QueryTypes;

namespace Searchle.GraphQL.Mappers
{
  public class DictionaryWordMapper : IObjectMapper<LexicalWord, DictionaryWord>, IObjectMapper<DictionaryWord, LexicalWord>
  {
    public LexicalWord Transform(DictionaryWord obj)
    {
      return new LexicalWord
      {
        LexicalWordId = obj.Id,
        Lemma = obj.Word
      };
    }

    public DictionaryWord Transform(LexicalWord obj)
    {
      return new DictionaryWord
      {
        Id = obj.LexicalWordId,
        Word = obj.Lemma
      };
    }
  }
}
