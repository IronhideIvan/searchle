using System;
using Searchle.Common.Logging;
using Searchle.DataAccess.Postgres;

namespace Searchle.GraphQL.ApplicationStartup
{
  public class SearchleAppConfig
  {
    public AppLoggingConfig? Logging { get; set; }
    public PostgresConnectionConfig? DictionaryConnectionConfig { get; set; }
  }
}
