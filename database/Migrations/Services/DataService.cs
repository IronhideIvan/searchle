using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Migrations.Models;
using Npgsql;

namespace Migrations.Services
{
  internal sealed class DataService : IDataService
  {
    private readonly IEnumerable<DatabaseConnectionConfig> cnnConfigs;

    public DataService(AppConfig appConfig)
    {
      cnnConfigs = appConfig.Connections;
    }

    public string GetConnectionString()
    {
      var cnnConfig = cnnConfigs.First();
      string cnnString = $"Server={cnnConfig.Server};Port={cnnConfig.Port};Database={cnnConfig.Database};User Id={cnnConfig.Username};Password={cnnConfig.Password};";
      return cnnString;
    }

    public async Task<IDbConnection> ConnectAsync()
    {
      string cnnString = GetConnectionString();

      var cnn = new NpgsqlConnection(cnnString);
      await cnn.OpenAsync();
      return cnn;
    }
  }
}