using System.Collections.Generic;
using Searchle.Configuration.Interfaces;

namespace Migrations.Core.Models
{
  public class AppSettings
  {
    public AppConfig AppConfig { get; set; }
  }

  public class AppConfig
  {
    public IEnumerable<string> SetupScripts { get; set; }
    public IEnumerable<DatabaseConnectionConfig> Connections { get; set; }
    public DatabaseMetadata Metadata { get; set; }
  }

  public class DatabaseConnectionConfig
  {
    public ISecret ConnectionString { get; set; }
  }

  public class DatabaseMetadata
  {
    public string Catalog { get; set; }
    public string Schema { get; set; }
  }
}