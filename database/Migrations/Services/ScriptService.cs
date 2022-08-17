using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Migrations.Errors;
using Migrations.Models;

namespace Migrations.Services
{
  public class ScriptService : IScriptService
  {
    private readonly AppConfig _config;

    public ScriptService(AppConfig config)
    {
      _config = config;
    }

    public async Task<string> GetScriptAsync(ScriptKeys key)
    {
      var scriptConfig = _config.Scripts.FirstOrDefault(s => s.Key == key.ToString());

      if (scriptConfig == null)
      {
        throw new MigrationException($"Script with key '{key}' not found.");
      }

      var contents = await File.ReadAllTextAsync(scriptConfig.Filepath);
      return contents;
    }
  }
}
