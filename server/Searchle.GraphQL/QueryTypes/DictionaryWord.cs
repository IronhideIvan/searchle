namespace Searchle.GraphQL.QueryTypes
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
    public IEnumerable<DictionaryWordDefinition>? Definitions { get; set; }

    public static DictionaryWord Get(int Id)
    {
      return new DictionaryWord { Id = Id, Word = "test" };
    }
  }
}
