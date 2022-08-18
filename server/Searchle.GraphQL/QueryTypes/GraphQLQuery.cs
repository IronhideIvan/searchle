using System;

namespace Searchle.GraphQL.QueryTypes
{
  [GraphQLName("Query")]
  public class GraphQLQuery
  {
    public DictionaryWord GetWord() => new DictionaryWord { Id = 17, Word = "Test" };
  }
}
