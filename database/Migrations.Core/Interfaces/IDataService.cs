using System;
using System.Data;
using System.Threading.Tasks;

namespace Migrations.Core.Interfaces
{
  public interface IDataService
  {
    Task<string> GetConnectionStringAsync();
    Task<IDbConnection> ConnectAsync();
  }
}
