using System;
using Searchle.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Searchle.GraphQL.Schema.QueryTypes;

namespace Searchle.GraphQL.Mappers
{
  public class DictionaryWordDefinitionMapper
    : IObjectMapper<LexicalDefinition, DictionaryWordDefinition>,
      IObjectMapper<DictionaryWordDefinition, LexicalDefinition>
  {
    public DictionaryWordDefinition Transform(LexicalDefinition obj)
    {
      return new DictionaryWordDefinition
      {
        Id = obj.LexicalDefinitionId,
        WordId = obj.LexicalWordId,
        Definition = obj.Definition,
        Category = obj.Category,
        PartOfSpeech = obj.PartOfSpeech
      };
    }

    public LexicalDefinition Transform(DictionaryWordDefinition obj)
    {
      return new LexicalDefinition
      {
        LexicalWordId = obj.Id,
        LexicalDefinitionId = obj.WordId,
        Definition = obj.Definition,
        Category = obj.Category,
        PartOfSpeech = obj.PartOfSpeech
      };
    }
  }
}
