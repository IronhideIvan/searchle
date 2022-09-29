using System;

namespace Searchle.DataAccess.Common.Interfaces
{
  public interface IQuery
  {
    string BuildQuery();
    object GetParameters();
  }

  public interface IQuery<T> : IQuery
  {

  }
}
