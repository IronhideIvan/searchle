using System;

namespace Searchle.Dictionary.Common.Models
{
  public enum EqualityType
  {
    GreaterThan,
    LessThan,
    GreaterThanOrEqualTo,
    LessThanOrEqualTo,
    EqualTo
  }

  public class LexicalSearchInstanceCount
  {
    public char Letter { get; set; }
    public int Count { get; set; }
    public EqualityType Equality { get; set; }
  }
}
