using Searchle.Common.Logging;
using Serilog;

namespace Searchle.GraphQL.Logging
{
  public class SerilogLoggerFactory : IAppLoggerFactory
  {
    private AppLoggingConfig _config;
    private Serilog.ILogger _logger;

    public SerilogLoggerFactory(AppLoggingConfig config)
    {
      _config = config;

      _logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();
    }

    public IAppLogger<T> Create<T>()
    {
      return new SerilogLogger<T>(_config, _logger);
    }
  }
}
