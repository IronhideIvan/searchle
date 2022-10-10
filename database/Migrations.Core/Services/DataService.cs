using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Migrations.Core.Interfaces;
using Migrations.Core.Models;
using Npgsql;

namespace Migrations.Core.Services
{
  internal sealed class DataService : IDataService
  {
    private readonly IEnumerable<DatabaseConnectionConfig> cnnConfigs;

    public DataService(AppConfig appConfig)
    {
      cnnConfigs = appConfig.Connections;
    }

    public async Task<string> GetConnectionStringAsync()
    {
      var cnnConfig = cnnConfigs.First();
      return await cnnConfig.ConnectionString.GetValueAsync();
    }

    public async Task<IDbConnection> ConnectAsync()
    {
      string cnnString = await GetConnectionStringAsync();

      var cnn = new NpgsqlConnection(cnnString);
      await cnn.OpenAsync();
      return cnn;
    }
  }
}