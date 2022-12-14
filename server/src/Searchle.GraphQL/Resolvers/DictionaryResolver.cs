using System;
using Searchle.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Searchle.Dictionary.Business.Services;
using Searchle.GraphQL.Schema;
using Searchle.GraphQL.Schema.QueryTypes;
using Searchle.Common.Logging;

namespace Searchle.GraphQL.Resolvers
{
  [ExtendObjectType(typeof(GraphQLQuery))]
  public class DictionaryResolver
  {
    [GraphQLName("word")]
    public async Task<DictionaryWord?> GetWord(
      [ID(nameof(DictionaryWord))] int id,
      [Service] ILexicalWordService service,
      [Service] IObjectTransformer<LexicalWord, DictionaryWord> mapper
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
      [Service] IObjectTransformer<LexicalDefinition, DictionaryWordDefinition> mapper
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
      [Service] IObjectTransformer<LexicalDefinition, DictionaryWordDefinition> mapper
    )
    {
      var definitions = await service.GetLexicalDefinitionsByWord(wordId);
      return definitions.Select(d => mapper.Transform(d));
    }

    [GraphQLName("wordSearch")]
    public async Task<IEnumerable<DictionaryWord>> GetWordSearch(
      string queryString,
      [Service] ILexicalSearchService searchService,
      [Service] IObjectTransformer<LexicalWord, DictionaryWord> wordTransformer,
      [Service] IObjectTransformer<string, LexicalSearch> queryTransformer,
      [Service] IAppLoggerFactory loggerFactory
      )
    {
      var logger = loggerFactory.Create<DictionaryResolver>();
      logger.Debug("Method: {MethodName}, Query: {QueryString}", nameof(GetWordSearch), queryString);

      if (string.IsNullOrWhiteSpace(queryString))
      {
        return new DictionaryWord[] { };
      }

      var parsedQuery = queryTransformer.Transform(queryString);
      var results = await searchService.SearchWordsAsync(parsedQuery);
      return results.Select(r => wordTransformer.Transform(r));
    }
  }
}
