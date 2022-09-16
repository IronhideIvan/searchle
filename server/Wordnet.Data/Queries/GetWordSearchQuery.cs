using System;
using System.Data;
using System.Text;
using Dapper;
using Searchle.DataAccess.Common;
using Searchle.DataAccess.Common.Interfaces;
using Searchle.DataAccess.Common.Sql;
using Searchle.Dictionary.Common.Models;

namespace Wordnet.Data.Queries
{
  public class GetWordSearchQuery : IQuery<LexicalWord>
  {
    private const int MAX_RESULT_LIMIT = 50;
    private const int MIN_RESULT_LIMIT = 5;
    private const int DEFAULT_RESULT_LIMIT = 25;

    public LexicalSearch SearchQuery = new LexicalSearch();
    public SelectorCollection? Selectors { get; set; }

    public string BuildQuery()
    {
      var query = new QueryBuilder()
        .From("public.word w")
        .AddSelect("w.wordid", nameof(LexicalWord.LexicalWordId))
        .AddSelectIfSelectorExists("w.lemma", nameof(LexicalWord.Lemma), Selectors);

      if (SearchQuery.LetterCount > 0)
      {
        query.AddWhere("length(w.lemma) = :LetterCount");
      }

      if (SearchQuery.ExcludeSpecialCharacters)
      {
        query.AddWhere("w.lemma !~* '[0-9\\s''\\-]'");
      }

      if (SearchQuery.MustInclude?.Count() > 0)
      {
        query.AddWhere("w.lemma ~* :MustInclude");
      }

      if (SearchQuery.MustExclude?.Count() > 0)
      {
        query.AddWhere("w.lemma !~* :MustExclude");
      }

      if (SearchQuery.SearchTerms?.Count() > 0)
      {
        int index = 0;
        foreach (var term in SearchQuery.SearchTerms)
        {
          query.AddWhere($"w.lemma like '%' || :Term{index} || '%'");
          ++index;
        }
      }

      if (SearchQuery.ExactSearch?.Count() > 0)
      {
        query.AddWhere("w.lemma like :ExactSearch");
      }

      if (SearchQuery.MustIncludeAtPosition?.Count() > 0)
      {
        int index = 0;
        foreach (var term in SearchQuery.MustIncludeAtPosition)
        {
          query.AddWhere($"w.lemma like :MustIncludeAtPosition{index}");
          ++index;
        }
      }

      if (SearchQuery.MustExcludeAtPosition?.Count() > 0)
      {
        int index = 0;
        foreach (var term in SearchQuery.MustExcludeAtPosition)
        {
          query.AddWhere($"w.lemma not like :MustExcludeAtPosition{index}");
          ++index;
        }
      }

      if (SearchQuery.InstanceCounts?.Count() > 0)
      {
        // ((length(w.lemma) - length(replace(w.lemma, 'bi', '')) )::int / length('bi')) > 1
        int index = 0;
        foreach (var instance in SearchQuery.InstanceCounts)
        {
          string equalityOperator = "=";
          switch (instance.Equality)
          {
            case EqualityType.EqualTo:
              equalityOperator = "=";
              break;
            case EqualityType.GreaterThan:
              equalityOperator = ">";
              break;
            case EqualityType.LessThan:
              equalityOperator = "<";
              break;
            case EqualityType.GreaterThanOrEqualTo:
              equalityOperator = ">=";
              break;
            case EqualityType.LessThanOrEqualTo:
              equalityOperator = "<=";
              break;
            default:
              throw new NotImplementedException($"Unknown EqualityType: {instance.Equality}");
          }

          query.AddWhere($"(length(w.lemma) - length(replace(w.lemma, :InstanceCount{index}, '')))::int {equalityOperator} {instance.Count}");
        }
      }

      int limit = SearchQuery.ResultLimit;
      if (SearchQuery.ResultLimit == default(int))
      {
        limit = DEFAULT_RESULT_LIMIT;
      }
      else if (SearchQuery.ResultLimit > MAX_RESULT_LIMIT)
      {
        limit = MAX_RESULT_LIMIT;
      }
      else if (SearchQuery.ResultLimit < MIN_RESULT_LIMIT)
      {
        limit = MIN_RESULT_LIMIT;
      }
      query.Limit(limit);

      return query.Build();
    }

    public object GetParameters()
    {
      var parameters = new DynamicParameters();

      if (SearchQuery.LetterCount > 0)
      {
        parameters.Add("LetterCount", SearchQuery.LetterCount, DbType.Int32);
      }

      if (SearchQuery.MustInclude?.Count() > 0)
      {
        var sb = new StringBuilder();
        foreach (var c in SearchQuery.MustInclude)
        {
          sb.Append($"(?=.*{GetRegexSanitizedChar(c)})");
        }
        parameters.Add("MustInclude", sb.ToString());
      }

      if (SearchQuery.MustExclude?.Count() > 0)
      {
        var sb = new StringBuilder("[");
        foreach (var c in SearchQuery.MustExclude)
        {
          sb.Append(GetRegexSanitizedChar(c));
        }
        sb.Append("]");
        parameters.Add("MustExclude", sb.ToString());
      }

      if (SearchQuery.SearchTerms?.Count() > 0)
      {
        int index = 0;
        foreach (var term in SearchQuery.SearchTerms)
        {
          parameters.Add($"Term{index}", term);
          ++index;
        }
      }

      if (SearchQuery.ExactSearch?.Count() > 0)
      {
        var sb = new StringBuilder();
        foreach (var c in SearchQuery.ExactSearch)
        {
          if (char.IsLetterOrDigit(c))
          {
            sb.Append(c);
          }
          else
          {
            sb.Append("_");
          }
        }
        parameters.Add($"ExactSearch", sb.ToString());
      }

      if (SearchQuery.MustIncludeAtPosition?.Count() > 0)
      {
        int index = 0;
        foreach (var term in SearchQuery.MustIncludeAtPosition)
        {
          var clause = GetPositionalSearchTerm(term);
          parameters.Add($"MustIncludeAtPosition{index}", clause);
          ++index;
        }
      }

      if (SearchQuery.MustExcludeAtPosition?.Count() > 0)
      {
        int index = 0;
        foreach (var term in SearchQuery.MustExcludeAtPosition)
        {
          var clause = GetPositionalSearchTerm(term);
          parameters.Add($"MustExcludeAtPosition{index}", clause);
          ++index;
        }
      }

      if (SearchQuery.InstanceCounts?.Count() > 0)
      {
        int index = 0;
        foreach (var instance in SearchQuery.InstanceCounts)
        {
          parameters.Add($"InstanceCount{index}", instance.Letter.ToString());
          ++index;
        }
      }

      return parameters;
    }

    private string GetPositionalSearchTerm(LexicalSearchSpecificPosition term)
    {
      var sb = new StringBuilder();
      for (int i = 0; i < term.Position; ++i)
      {
        sb.Append("_");
      }

      sb.Append(term.Letter + "%");
      return sb.ToString();
    }

    private string GetRegexSanitizedChar(char input)
    {
      if (Char.IsLetterOrDigit(input))
      {
        return input.ToString();
      }
      else if (input == '-')
      {
        return "\\-";
      }
      else if (char.IsWhiteSpace(input))
      {
        return "\\s";
      }

      return string.Empty;
    }
  }
}
