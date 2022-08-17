using System;

namespace Migrations.Core.Errors
{
  public class MigrationException : Exception
  {
    public MigrationException(string message) : base(message)
    {

    }
  }
}
