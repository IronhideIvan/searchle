using Searchle.Common.Interfaces;
using Searchle.Dictionary.Common.Models;
using Searchle.Dictionary.Data.Services;

namespace Searchle.GraphQL.Schema.QueryTypes
{
  [Node]
  [GraphQLName("DictionaryWordDefinition")]
  public class DictionaryWordDefinition
  {
    [ID]
    [GraphQLName("id")]
    public int Id { get; set; }

    [GraphQLName("dictionaryWordId")]
    public int WordId { get; set; }

    [GraphQLName("definition")]
    public string? Definition { get; set; }

    [GraphQLName("pos")]
    public string? PartOfSpeech { get; set; }

    [GraphQLName("category")]
    public string? Category { get; set; }

    public static async Task<DictionaryWordDefinition?> GetAsync(int id,
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
  }
}
