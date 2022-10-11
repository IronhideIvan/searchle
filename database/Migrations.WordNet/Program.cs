using Migrations.Core;

internal class Program
{
  private static async Task Main(string[] args)
  {
    var migrator = new Migrator<Program>();
    await migrator.Run(typeof(Program).Assembly);
  }
}