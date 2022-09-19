using System;
using System.Data;
using Dapper;
using Npgsql;
using Searchle.Common.Logging;
using Searchle.DataAccess.Common.Interfaces;

namespace Searchle.DataAccess.Postgres
{
  public class PostgresDataProvider : IDataProvider
  {
    private PostgresConnectionConfig _config;
    private IAppLogger<PostgresDataProvider> _logger;

    public PostgresDataProvider(PostgresConnectionConfig config, IAppLoggerFactory loggerFactory)
    {
      _config = config;
      _logger = loggerFactory.Create<PostgresDataProvider>();
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(IQuery<T> query)
    {
      var sql = query.BuildQuery();

      using (var cnn = new NpgsqlConnection(_config.ConnectionString))
      {
        cnn.Open();
        _logger.Debug("Executing SQL Statement '{sql}'", sql);
        var results = await cnn.QueryAsync<T>(sql, param: query.GetParameters());
        return results;
      }
    }
  }
}
