using System.Collections.Generic;

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
    public string Environment { get; set; }
    public string Server { get; set; }
    public int Port { get; set; }
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
  }

  public class DatabaseMetadata
  {
    public string Catalog { get; set; }
    public string Schema { get; set; }
  }
}