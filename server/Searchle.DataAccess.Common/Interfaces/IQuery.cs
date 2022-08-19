using System;

namespace Searchle.DataAccess.Common.Interfaces
{
  public interface IQuery<T>
  {
    string BuildQuery();
  }
}
