using System;
using Searchle.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Searchle.Dictionary.Data.Services;
using Searchle.GraphQL.Schema;
using Searchle.GraphQL.Schema.QueryTypes;

namespace Searchle.GraphQL.Resolvers
{
  [ExtendObjectType(typeof(GraphQLQuery))]
  public class DictionaryWordResolver
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
  }
}
