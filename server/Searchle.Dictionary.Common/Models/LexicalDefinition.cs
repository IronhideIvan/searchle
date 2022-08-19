using System;

namespace Searchle.Dictionary.Common.Models
{
  public class LexicalDefinition
  {
    public int LexicalDefinitionId { get; set; }
    public int LexicalWordId { get; set; }
    public string? Definition { get; set; }
  }
}
