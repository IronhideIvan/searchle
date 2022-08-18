using System;

namespace Searchle.GraphQl.GraphQlTypes
{
  public class GraphQLQuery
  {
    public DictionaryWordGQLType GetWord() => new DictionaryWordGQLType { Word = "Test" };
  }
}
