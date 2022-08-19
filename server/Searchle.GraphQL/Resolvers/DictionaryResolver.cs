using System;
using Searchle.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Searchle.Dictionary.Data.Services;
using Searchle.GraphQL.Schema;
using Searchle.GraphQL.Schema.QueryTypes;

namespace Searchle.GraphQL.Resolvers
{
  [ExtendObjectType(typeof(GraphQLQuery))]
  public class DictionaryResolver
  {
    [GraphQLName("word")]
    public async Task<DictionaryWord?> GetWord(
      [ID(nameof(DictionaryWord))] int id,
      [Service] ILexicalWordService service,
      [Service] IObjectMapper<LexicalWord, DictionaryWord> mapper
    )
    {
      var word = await service.GetWordAsync(id);
      if (word == null)
      {
        return null;
      }

      return mapper.Transform(word);
    }

    [GraphQLName("definition")]
    public async Task<DictionaryWordDefinition?> GetSingleDefinition(
      [ID(nameof(DictionaryWordDefinition))] int id,
      [Service] ILexicalDefinitionService service,
      [Service] IObjectMapper<LexicalDefinition, DictionaryWordDefinition> mapper
      )
    {
      var definition = await service.GetLexicalDefinition(id);
      if (definition == null)
      {
        return null;
      }
      return mapper.Transform(definition);
    }

    [GraphQLName("definitions")]
    public async Task<IEnumerable<DictionaryWordDefinition>> GetDefinitionsByWord(
      [ID(nameof(DictionaryWord))] int wordId,
      [Service] ILexicalDefinitionService service,
      [Service] IObjectMapper<LexicalDefinition, DictionaryWordDefinition> mapper
    )
    {
      var definitions = await service.GetLexicalDefinitionsByWord(wordId);
      return definitions.Select(d => mapper.Transform(d));
    }
  }
}
