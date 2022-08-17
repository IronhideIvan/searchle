using System.Collections.Generic;

namespace Migrations.Models
{
  public class AppSettings
  {
    public AppConfig AppConfig { get; set; }
  }

  public class AppConfig
  {
    public IEnumerable<ScriptConfig> Scripts { get; set; }
    public IEnumerable<DatabaseConnectionConfig> Connections { get; set; }
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

  public class ScriptConfig
  {
    public string Key { get; set; }
    public string Filepath { get; set; }
  }

  public enum ScriptKeys
  {
    CreateDatabase,
    CreateTables,
    SeedData
  }
}