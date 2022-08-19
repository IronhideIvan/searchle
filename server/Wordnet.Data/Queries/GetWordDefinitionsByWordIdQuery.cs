using System;
using System.Text;
using Searchle.DataAccess.Common;
using Searchle.DataAccess.Common.Interfaces;
using Searchle.DataAccess.Common.Sql;
using Searchle.Dictionary.Common.Models;

namespace Wordnet.Data.Queries
{
  internal class GetWordDefinitionsByWordIdQuery : IQuery<LexicalDefinition>
  {
    public int WordId { get; set; }
    public SelectorCollection? Selectors { get; set; }

    public string BuildQuery()
    {
      var query = new QueryBuilder()
          .AddSelect("def.definition", nameof(LexicalDefinition.Definition))
          .AddSelectIfSelectorExists("def.synsetid", nameof(LexicalDefinition.LexicalDefinitionId), this.Selectors)
          .AddSelectIfSelectorExists("def.pos", nameof(LexicalDefinition.PartOfSpeech), this.Selectors)
          .AddSelectIfSelectorExists("c.name", nameof(LexicalDefinition.Category), this.Selectors)
          .From("public.synset def")
          .AddJoin(new JoinBuilder(JoinType.Inner, "public.sense sen")
            .AddOn("sen.synsetid = def.synsetid"))
          .AddJoin(new JoinBuilder(JoinType.Inner, "public.word w")
            .AddOn("w.wordid = sen.wordid"))
          .AddJoin(new JoinBuilder(JoinType.Inner, "public.categorydef c")
            .AddOn("c.categoryid = def.categoryid"))
          .AddWhere("w.wordid = :WordId")
          .Build();

      return query;
    }
  }
}
