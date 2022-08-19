using System;
using System.Collections;
using Searchle.Common.Exceptions;

namespace Searchle.DataAccess.Common
{
  public class SelectorCollection : IEnumerable<string>
  {
    private HashSet<string> _selectorCol;

    public SelectorCollection(params string[] selectors)
    {
      _selectorCol = new HashSet<string>();

      if (selectors != null)
      {
        foreach (var s in selectors)
        {
          Add(s);
        }
      }
    }

    public void Add(string selector)
    {
      if (_selectorCol.Contains(selector))
      {
        throw new SearchleDomainException($"Selector with name '{selector}' already exists within the collection. Duplicate selectors are not allowed.");
      }

      _selectorCol.Add(selector);
    }

    public bool Contains(string selector)
    {
      return _selectorCol.Contains(selector);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return _selectorCol.GetEnumerator();
    }

    IEnumerator<string> IEnumerable<string>.GetEnumerator()
    {
      return _selectorCol.GetEnumerator();
    }
  }
}
