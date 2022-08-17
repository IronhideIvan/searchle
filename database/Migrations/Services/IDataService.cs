using System;
using System.Data;
using System.Threading.Tasks;

namespace Migrations.Services
{
  public interface IDataService
  {
    string GetConnectionString();
    Task<IDbConnection> ConnectAsync();
  }
}
