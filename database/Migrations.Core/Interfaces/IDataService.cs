using System;
using System.Data;
using System.Threading.Tasks;

namespace Migrations.Core.Interfaces
{
  public interface IDataService
  {
    string GetConnectionString();
    Task<IDbConnection> ConnectAsync();
  }
}
