using System;
using System.Globalization;
using Searchle.Common.Interfaces;
using Searchle.Common.Logging;
using Searchle.Dictionary.Common.Models;

namespace Searchle.GraphQL.Transformers
{
  public class WordSearchTransformer : IObjectTransformer<string, LexicalSearch>
  {
    private const string QUERY_CLAUSE_SEPARATOR = ":";

    private IAppLogger<WordSearchTransformer> _logger;

    public WordSearchTransformer(IAppLoggerFactory loggerFactory)
    {
      _logger = loggerFactory.Create<WordSearchTransformer>();
    }

    /*
      example query with explanations:

      "di l:5 in:abc pos:a|2,b|3,c|1 dif:d|2,e|3 ex:ef"

      di = word contains a segment with "di".
      l:5 = word has a length of 5 letters.
      in:abc = word must include the letters "a", "b", and "c", 
            regardless of position.
      pos:a|2,b|3,c|1 = word must include the letters "a", "b" and "c".
            However, the "a" is in the 2nd position, the "b" in 3rs, etc.
      dif:d|2,e|3 = word includes the letters "d" and "e". However, "d"
            is not in 2nd position and "e" is not in 3rd.
      ex:ef = word must exclude the letters "e" and "f".
    */
    public LexicalSearch Transform(string searchQuery)
    {
      var searchTerms = new List<string>();
      var mustInclude = new List<char>();
      var mustExclude = new List<char>();
      var exactSearch = new List<char>();
      var mustIncludeAtPosition = new List<LexicalSearchSpecificPosition>();
      var mustExcludeAtPosition = new List<LexicalSearchSpecificPosition>();
      var instanceCounts = new List<LexicalSearchInstanceCount>();

      var searchObj = new LexicalSearch
      {
        SearchTerms = searchTerms,
        MustExclude = mustExclude,
        MustInclude = mustInclude,
        ExactSearch = exactSearch,
        MustIncludeAtPosition = mustIncludeAtPosition,
        MustExcludeAtPosition = mustExcludeAtPosition,
        InstanceCounts = instanceCounts
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
          case "r":
            int limit = 0;
            Int32.TryParse(segmentValue, out limit);
            searchObj.ResultLimit = limit;
            break;
          case "in":
            mustInclude.AddRange(segmentValue.ToCharArray());
            break;
          case "ex":
            mustExclude.AddRange(segmentValue.ToCharArray());
            break;
          case "es":
            exactSearch.AddRange(segmentValue.ToCharArray());
            break;
          case "pos":
            AddToPositionList(segmentValue, mustIncludeAtPosition);
            break;
          case "dif":
            AddToPositionList(segmentValue, mustExcludeAtPosition);
            break;
          case "cnt":
            AddToInstanceCountList(segmentValue, instanceCounts);
            break;
          case "sp":
            if (segmentValue == "y")
            {
              searchObj.ExcludeSpecialCharacters = true;
            }
            else
            {
              searchObj.ExcludeSpecialCharacters = false;
            }
            break;
          default:
            searchTerms.Add(seg);
            break;
        }
      }

      return searchObj;
    }

    private void AddToInstanceCountList(string searchClause, List<LexicalSearchInstanceCount> list)
    {
      var segments = searchClause?.Split(",");

      if (segments == null || segments.Length == 0)
      {
        return;
      }

      foreach (var segment in segments)
      {
        // Arbitrary sanity check so that we don't filter too many things in one
        // word and possibly break the query.
        if (list.Count > 10)
        {
          break;
        }

        // [0] = character
        // [1] = equality operator
        // [2] = count
        var posClause = segment.Split("|");
        if (posClause.Length < 3)
        {
          continue;
        }

        if (!char.TryParse(posClause[0].Trim(), out char c))
        {
          continue;
        }

        var equalityOperator = posClause[1].Trim();
        EqualityType equalityType;
        switch (equalityOperator)
        {
          case "=":
            equalityType = EqualityType.EqualTo;
            break;
          case ">":
            equalityType = EqualityType.GreaterThan;
            break;
          case "<":
            equalityType = EqualityType.LessThan;
            break;
          case ">=":
            equalityType = EqualityType.GreaterThanOrEqualTo;
            break;
          case "<=":
            equalityType = EqualityType.LessThanOrEqualTo;
            break;
          default:
            // Could not be found.
            continue;
        }


        if (!Int32.TryParse(posClause[2].Trim(), out int count))
        {
          continue;
        }

        // count smaller than 0 is nonsensical, ignore this statement.
        if (count < 0)
        {
          continue;
        }

        list.Add(new LexicalSearchInstanceCount
        {
          Letter = c,
          Count = count,
          Equality = equalityType
        });
      }
    }

    private void AddToPositionList(string searchClause, List<LexicalSearchSpecificPosition> list)
    {
      var segments = searchClause?.Split(",");

      if (segments == null || segments.Length == 0)
      {
        return;
      }

      foreach (var segment in segments)
      {
        // Arbitrary sanity check so that we don't filter too many things in one
        // word and possibly break the query.
        if (list.Count > 20)
        {
          break;
        }

        // [0] = character
        // [1] = position
        var posClause = segment.Split("|");
        if (posClause.Length < 2)
        {
          continue;
        }

        if (!char.TryParse(posClause[0].Trim(), out char c))
        {
          continue;
        }

        if (!Int32.TryParse(posClause[1].Trim(), out int pos))
        {
          continue;
        }

        // Arbitrary sanity so that we don't try to break the underlying query with words too long
        if (pos < 0 || pos > 50)
        {
          continue;
        }

        list.Add(new LexicalSearchSpecificPosition
        {
          Letter = c,
          Position = pos
        });
      }
    }
  }
}
