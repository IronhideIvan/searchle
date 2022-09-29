using System;
using System.Text;

namespace Searchle.DataAccess.Common.Sql
{
  public class QueryBuilder
  {
    private int _limit { get; set; }
    private string? _fromClause { get; set; }

    private List<string> _selectStatements = new List<string>();
    private List<string> _whereClauses = new List<string>();
    private List<JoinBuilder>? _joins;

    public QueryBuilder AddSelect(string dbField, string alias)
    {
      _selectStatements.Add($"{dbField} as \"{alias}\"");
      return this;
    }

    public QueryBuilder AddSelectIfSelectorExists(string dbField, string alias, SelectorCollection? selectors)
    {
      if (selectors == null || selectors.Contains(alias))
      {
        AddSelect(dbField, alias);
      }

      return this;
    }

    public QueryBuilder AddWhere(string clause)
    {
      _whereClauses.Add(clause);
      return this;
    }

    public QueryBuilder From(string table)
    {
      _fromClause = table;
      return this;
    }

    public QueryBuilder AddJoin(JoinBuilder join)
    {
      if (_joins == null)
      {
        _joins = new List<JoinBuilder>();
      }

      _joins.Add(join);
      return this;
    }

    public QueryBuilder Limit(int limit)
    {
      _limit = limit;
      return this;
    }

    public string Build()
    {
      var sb = new StringBuilder();

      // select
      sb.Append("select ");
      for (int i = 0; i < _selectStatements.Count; ++i)
      {
        if (i > 0)
        {
          sb.Append(", ");
        }

        sb.Append(_selectStatements[i]);
      }

      // from
      sb.AppendLine();
      sb.Append($"from {_fromClause}");

      // join
      if (_joins != null && _joins.Count > 0)
      {
        sb.AppendLine();
        foreach (var join in _joins)
        {
          sb.AppendLine(join.Build());
        }
      }

      // where
      for (int i = 0; i < _whereClauses.Count; ++i)
      {
        sb.AppendLine();
        if (i == 0)
        {
          sb.Append("where");
        }
        else
        {
          sb.Append("and");
        }

        sb.Append($" {_whereClauses[i]}");
      }

      // limit
      if (_limit > 0)
      {
        sb.AppendLine();
        sb.Append($"limit {_limit}");
      }

      sb.Append(";");

      return sb.ToString();
    }
  }
}
