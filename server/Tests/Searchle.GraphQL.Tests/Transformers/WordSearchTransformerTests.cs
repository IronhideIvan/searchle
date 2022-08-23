using System;
using Searchle.GraphQL.Transformers;

namespace Searchle.GraphQL.Tests.Services
{
  public class QueryParserServiceTests
  {
    private WordSearchTransformer _service;

    public QueryParserServiceTests()
    {
      _service = new WordSearchTransformer();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    [InlineData(":")]
    [InlineData("x:")]
    [InlineData(":bcs")]
    [InlineData(":: :: : : :: :.:;:")]
    public void WordSearchTransformer_ParseQueryString_InvalidQueriesDoNotThrow(string query)
    {
      var ret = _service.Transform(query);
      Assert.NotNull(ret);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData(null, 0)]
    [InlineData("l:3", 3)]
    [InlineData("abc l:7 in:1 ::: df ex:ioen", 7)]
    [InlineData("l:2 l:5 l:8", 8)]
    public void WordSearchTransformer_ParseQueryString_WordLengthParses(string query, int expectedLength)
    {
      var ret = _service.Transform(query);
      Assert.Equal(expectedLength, ret.LetterCount);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData(null, 0)]
    [InlineData("r:3", 3)]
    [InlineData("abc r:7 l:3 in:1 ::: df ex:ioen", 7)]
    [InlineData("l:2 r:5 r:8", 8)]
    public void WordSearchTransformer_ParseQueryString_ResultLimitParses(string query, int expectedLength)
    {
      var ret = _service.Transform(query);
      Assert.Equal(expectedLength, ret.ResultLimit);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("l:3")]
    [InlineData("in:a", 'a')]
    [InlineData("in:aBc", 'a', 'b', 'c')]
    [InlineData("ex:ad :: in:2j7 l:4   4adf abc", '2', 'j', '7')]
    [InlineData("ex:ad :: in:2j7 l:4   4adf in:POL abc", '2', 'j', '7', 'p', 'o', 'l')]
    public void WordSearchTransformer_ParseQueryString_IncludesParse(string query, params char[] expectedCharacters)
    {
      var ret = _service.Transform(query);
      ValidateValuesInCollection(ret.MustInclude, expectedCharacters);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("l:3")]
    [InlineData("ex:a", 'a')]
    [InlineData("ex:aBc", 'a', 'b', 'c')]
    [InlineData("in:ad :: ex:2j7 l:4   4adf abc", '2', 'j', '7')]
    [InlineData("in:ad :: ex:2J7 l:4   4adf ex:POL abc", '2', 'j', '7', 'p', 'o', 'l')]
    public void WordSearchTransformer_ParseQueryString_ExcludesParse(string query, params char[] expectedCharacters)
    {
      var ret = _service.Transform(query);
      ValidateValuesInCollection(ret.MustExclude, expectedCharacters);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("l:3")]
    [InlineData("es:a", 'a')]
    [InlineData("es:aBc", 'a', 'b', 'c')]
    [InlineData("ex:ad :: es:2j7 l:4   4adf abc", '2', 'j', '7')]
    [InlineData("ex:ad :: es:2j7 l:4   4adf es:POL abc", '2', 'j', '7', 'p', 'o', 'l')]
    [InlineData("es:a_b_c", 'a', '_', 'b', '_', 'c')]
    public void WordSearchTransformer_ParseQueryString_ExactSearchParses(string query, params char[] expectedCharacters)
    {
      var ret = _service.Transform(query);
      ValidateValuesInCollection(ret.ExactSearch, expectedCharacters);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("abc", "abc")]
    [InlineData("d", "d")]
    [InlineData("::", "::")]
    [InlineData("abc: :ef", "abc:", ":ef")]
    [InlineData("in:av l:3 ex:2 abc l:5 in:b 56", "abc", "56")]
    [InlineData("John Rambo", "john", "rambo")]
    [InlineData("a : b : c", "a", ":", "b", ":", "c")]
    public void WordSearchTransformer_ParseQueryString_SearchTermsParse(string query, params string[] expectedTerms)
    {
      var ret = _service.Transform(query);
      ValidateValuesInCollection(ret.SearchTerms, expectedTerms);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("abc", false)]
    [InlineData("sp:false", false)]
    [InlineData("sp:true", false)]
    [InlineData("sp:y", true)]
    [InlineData("sp:n sp:y", true)]
    [InlineData("sp:y sp:n", false)]
    public void WordSearchTransformer_ParseQueryString_ExcludeSpecialCharactersParses(string query, bool expectedValue)
    {
      var ret = _service.Transform(query);
      Assert.Equal(expectedValue, ret.ExcludeSpecialCharacters);
    }

    [Fact]
    public void WordSearchTransformer_ParseQueryString_FullQueryParses()
    {
      string query = "dr l:5 in:abc ex:ef sp:y es:d_i_e r:40";
      var ret = _service.Transform(query);
      ValidateValuesInCollection(ret.SearchTerms, new[] { "dr" });
      ValidateValuesInCollection(ret.MustInclude, new[] { 'a', 'b', 'c' });
      ValidateValuesInCollection(ret.MustExclude, new[] { 'e', 'f' });
      ValidateValuesInCollection(ret.ExactSearch, new[] { 'd', '_', 'i', '_', 'e' });
      Assert.Equal(5, ret.LetterCount);
      Assert.Equal(40, ret.ResultLimit);
      Assert.True(ret.ExcludeSpecialCharacters);
    }

    private void ValidateValuesInCollection<T>(IEnumerable<T>? collection, T[] expectedValues)
    {
      Assert.NotNull(collection);

      if (expectedValues == null || expectedValues.Length == 0)
      {
        Assert.Empty(collection);
        return;
      }

      Assert.NotEmpty(collection);
      Assert.Equal(expectedValues.Length, collection!.Count());

      foreach (var e in expectedValues)
      {
        Assert.True(collection!.Any(c => e!.Equals(c)), $"The value '{e}' was expected in the collection but was not found.");
      }
    }
  }
}
