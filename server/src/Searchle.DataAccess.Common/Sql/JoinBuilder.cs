using System;
using System.Text;

namespace Searchle.DataAccess.Common.Sql
{
  public class JoinBuilder
  {
    private JoinType _joinType { get; set; }
    private string? _table { get; set; }

    private List<string> _onClauses = new List<string>();

    public JoinBuilder(JoinType joinType, string table)
    {
      _joinType = joinType;
      _table = table;
    }

    public JoinBuilder AddOn(string clause)
    {
      _onClauses.Add(clause);
      return this;
    }

    public string Build()
    {
      var sb = new StringBuilder();

      switch (_joinType)
      {
        default:
          sb.Append("inner join");
          break;
      }

      sb.Append($" {_table}");

      for (int i = 0; i < _onClauses.Count; ++i)
      {
        if (i == 0)
        {
          sb.Append(" on");
        }
        else
        {
          sb.AppendLine();
          sb.Append("and");
        }

        sb.Append($" {_onClauses[i]}");
      }

      return sb.ToString();
    }
  }
}
