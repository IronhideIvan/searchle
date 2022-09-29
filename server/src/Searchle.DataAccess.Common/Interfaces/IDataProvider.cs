using System;

namespace Searchle.DataAccess.Common.Interfaces
{
  public interface IDataProvider
  {
    Task<IEnumerable<T>> QueryAsync<T>(IQuery<T> query);
  }
}
