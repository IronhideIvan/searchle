using System;
using System.Text;
using Searchle.DataAccess.Common;
using Searchle.DataAccess.Common.Interfaces;
using Searchle.DataAccess.Common.Sql;
using Searchle.Dictionary.Common.Models;

namespace Wordnet.Data.Queries
{
  internal class GetWordQuery : IQuery<LexicalWord>
  {
    public int WordId { get; set; }
    public SelectorCollection? Selectors { get; set; }

    public string BuildQuery()
    {
      /*
        select 
            wordid,
            lemma
        from public.word w
        where w.lemma = 'pixel'
        limit 1;
      */

      var query = new QueryBuilder()
          .From("public.word w")
          .AddSelect("w.wordid", nameof(LexicalWord.LexicalWordId))
          .AddSelectIfSelectorExists("w.lemma", nameof(LexicalWord.Lemma), this.Selectors)
          .AddWhere("w.wordid = :WordId")
          .Limit(1)
          .Build();

      return query;
    }

    public object GetParameters()
    {
      return this;
    }
  }
}
