using System;

namespace Migrations.Errors
{
  public class MigrationException : Exception
  {
    public MigrationException(string message) : base(message)
    {

    }
  }
}
