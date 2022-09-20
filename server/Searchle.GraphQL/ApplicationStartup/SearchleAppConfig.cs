using System;
using Searchle.Common.Logging;
using Searchle.DataAccess.Postgres;

namespace Searchle.GraphQL.ApplicationStartup
{
  public class SearchleAppConfig
  {
    public AppMetadata Metadata { get; set; } = new AppMetadata();
    public AppLoggingConfig? Logging { get; set; }
    public PostgresConnectionConfig? DictionaryConnectionConfig { get; set; }
  }
}
