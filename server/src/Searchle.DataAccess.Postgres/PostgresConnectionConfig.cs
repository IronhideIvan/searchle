using System;
using Searchle.Common.Configuration;

namespace Searchle.DataAccess.Postgres
{
  public class PostgresConnectionConfig
  {
    public ISecret? ConnectionString { get; set; }
  }
}
