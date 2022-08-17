using System.Threading.Tasks;
using Migrations.Models;

namespace Migrations.Services
{
    public interface IScriptService
    {
        Task<string> GetScriptAsync(ScriptKeys key);
    }
}
