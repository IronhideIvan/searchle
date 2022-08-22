using System;
using System.Data;
using Dapper;
using Npgsql;
using Searchle.DataAccess.Common.Interfaces;

namespace Searchle.DataAccess.Postgres
{
  public class PostgresDataProvider : IDataProvider
  {
    private PostgresConnectionConfig _config;

    public PostgresDataProvider(PostgresConnectionConfig config)
    {
      _config = config;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(IQuery<T> query)
    {
      var sql = query.BuildQuery();

      using (var cnn = new NpgsqlConnection(_config.ConnectionString))
      {
        cnn.Open();
        var results = await cnn.QueryAsync<T>(sql, param: query.GetParameters());
        return results;
      }
    }
  }
}
