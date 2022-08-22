using System;
using Searchle.GraphQL.Services;

namespace Searchle.GraphQL.Tests.Services
{
  public class QueryParserServiceTests
  {
    private QueryParserService _service;

    public QueryParserServiceTests()
    {
      _service = new QueryParserService();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("      ")]
    [InlineData(":")]
    [InlineData("x:")]
    [InlineData(":bcs")]
    [InlineData(":: :: : : :: :.:;:")]
    public void QueryParserService_ParseQueryString_InvalidQueriesDoNotThrow(string query)
    {
      var ret = _service.ParseQueryString(query);
      Assert.NotNull(ret);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData(null, 0)]
    [InlineData("l:3", 3)]
    [InlineData("abc l:7 in:1 ::: df ex:ioen", 7)]
    [InlineData("l:2 l:5 l:8", 8)]
    public void QueryParserService_ParseQueryString_WordLengthParses(string query, int expectedLength)
    {
      var ret = _service.ParseQueryString(query);
      Assert.Equal(expectedLength, ret.LetterCount);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("l:3")]
    [InlineData("in:a", 'a')]
    [InlineData("in:aBc", 'a', 'b', 'c')]
    [InlineData("ex:ad :: in:2j7 l:4   4adf abc", '2', 'j', '7')]
    [InlineData("ex:ad :: in:2j7 l:4   4adf in:POL abc", '2', 'j', '7', 'p', 'o', 'l')]
    public void QueryParserService_ParseQueryString_IncludesParse(string query, params char[] expectedCharacters)
    {
      var ret = _service.ParseQueryString(query);
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
    public void QueryParserService_ParseQueryString_ExcludesParse(string query, params char[] expectedCharacters)
    {
      var ret = _service.ParseQueryString(query);
      ValidateValuesInCollection(ret.MustExclude, expectedCharacters);
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
    public void QueryParserService_ParseQueryString_SearchTermsParse(string query, params string[] expectedTerms)
    {
      var ret = _service.ParseQueryString(query);
      ValidateValuesInCollection(ret.SearchTerms, expectedTerms);
    }

    [Fact]
    public void QueryParserService_ParseQueryString_FullQueryParses()
    {
      string query = "dr l:5 in:abc ex:ef";
      var ret = _service.ParseQueryString(query);
      ValidateValuesInCollection(ret.SearchTerms, new[] { "dr" });
      ValidateValuesInCollection(ret.MustInclude, new[] { 'a', 'b', 'c' });
      ValidateValuesInCollection(ret.MustExclude, new[] { 'e', 'f' });
      Assert.Equal(5, ret.LetterCount);
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
