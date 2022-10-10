using System;
using Searchle.Configuration.Interfaces;

namespace Searchle.DataAccess.Postgres
{
  public class PostgresConnectionConfig
  {
    public ISecret? ConnectionString { get; set; }
  }
}
