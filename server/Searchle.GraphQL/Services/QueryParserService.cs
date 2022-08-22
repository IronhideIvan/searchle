using System;
using System.Globalization;
using Searchle.Dictionary.Common.Models;

namespace Searchle.GraphQL.Services
{
  public class QueryParserService : IQueryParserService
  {
    private const string QUERY_CLAUSE_SEPARATOR = ":";

    /*
      example query with explanations:

      "di l:5 in:abc ex:ef"

      dr = word contains a segment with "di".
      l:5 = word has a length of 5 letters.
      in:abc = word must include the letters "a", "b" and "c".
      ex:ef = word must exclude the letters "e" and "f".
    */
    public LexicalSearch ParseQueryString(string searchQuery)
    {
      var searchTerms = new List<string>();
      var mustInclude = new List<char>();
      var mustExclude = new List<char>();

      var searchObj = new LexicalSearch
      {
        SearchTerms = searchTerms,
        MustExclude = mustExclude,
        MustInclude = mustInclude
      };

      if (searchQuery == null || searchQuery.Length == 0)
      {
        return searchObj;
      }

      var sanitizedQuery = searchQuery?.Trim().ToLowerInvariant() ?? string.Empty;

      // split the query into segments
      var segments = sanitizedQuery.Split(" ");
      if (segments == null)
      {
        return searchObj;
      }

      // check each segment
      foreach (var seg in segments)
      {
        if (seg.Length == 0)
        {
          continue;
        }

        // check if the segment has a type separator. If it doesn't then
        // just treat the segment as a regular search term.
        int indexOfSeparator = seg.IndexOf(QUERY_CLAUSE_SEPARATOR);

        // There is no separator
        if (indexOfSeparator < 0
          // There is no type definition
          || indexOfSeparator == 0
          // There is no value after the separator
          || indexOfSeparator + 1 == seg.Length)
        {
          searchTerms.Add(seg);
          continue;
        }

        string segmentType = seg.Substring(0, indexOfSeparator);
        string segmentValue = seg.Substring(indexOfSeparator + 1);

        switch (segmentType)
        {
          case "l":
            int letterCount = 0;
            Int32.TryParse(segmentValue, out letterCount);
            searchObj.LetterCount = letterCount;
            break;
          case "in":
            mustInclude.AddRange(segmentValue.ToCharArray());
            break;
          case "ex":
            mustExclude.AddRange(segmentValue.ToCharArray());
            break;
          default:
            searchTerms.Add(seg);
            break;
        }
      }

      return searchObj;
    }
  }
}
