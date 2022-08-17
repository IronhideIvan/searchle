using System.Threading.Tasks;

namespace Migrations.Services
{
  public interface IDataaseMigrator
  {
    void MigrateUp(long? version = null);
    void MigrateDown(long version);
    Task EnsureDatabaseCreated();
  }
}