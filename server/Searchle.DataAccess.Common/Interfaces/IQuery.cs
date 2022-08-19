using System;

namespace Searchle.DataAccess.Common.Interfaces
{
  public interface IQuery
  {
    string BuildQuery();
  }

  public interface IQuery<T> : IQuery
  {

  }
}
