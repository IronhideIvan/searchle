namespace Searchle.GraphQL.Schema.QueryTypes
{
  [Node]
  [GraphQLName("DictionaryWordDefinition")]
  public class DictionaryWordDefinition
  {
    [ID]
    [GraphQLName("id")]
    public int Id { get; set; }

    [GraphQLName("definition")]
    public string? Definition { get; set; }

    public static DictionaryWordDefinition Get(int Id)
    {
      return new DictionaryWordDefinition { Id = Id, Definition = "A test definition." };
    }
  }
}
