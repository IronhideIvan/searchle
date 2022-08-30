using Searchle.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Searchle.Dictionary.Business.Services;
using Searchle.GraphQL.Schema.QueryTypes;

namespace Searchle.GraphQL.Schema.QueryTypes
{
  [Node]
  [GraphQLName("DictionaryWord")]
  public class DictionaryWord
  {
    [ID]
    [GraphQLName("id")]
    public int Id { get; set; }

    [GraphQLName("word")]
    public string? Word { get; set; }

    [GraphQLName("definitions")]
    public async Task<IEnumerable<DictionaryWordDefinition>>? GetDefinitions(
      [Service] ILexicalDefinitionService service,
      [Service] IObjectTransformer<LexicalDefinition, DictionaryWordDefinition> mapper
      )
    {
      var definitions = await service.GetLexicalDefinitionsByWord(this.Id);
      return definitions.Select(d => mapper.Transform(d));
    }

    public static async Task<DictionaryWord?> GetAsync(
      int id,
      [Service] ILexicalWordService service,
      [Service] IObjectTransformer<LexicalWord, DictionaryWord> mapper)
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
