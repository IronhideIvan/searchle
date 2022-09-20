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

      var serilogConfig = GetLoggerConfiguration(config);
      _logger = serilogConfig.CreateLogger();
    }

    public IAppLogger<T> Create<T>()
    {
      return new SerilogLogger<T>(_config, _logger);
    }

    private LoggerConfiguration GetLoggerConfiguration(AppLoggingConfig config)
    {
      var serilogConfig = new LoggerConfiguration();

      switch (config.LogLevel)
      {
        case AppLogLevel.Debug:
          serilogConfig = serilogConfig.MinimumLevel.Debug();
          break;
        case AppLogLevel.Information:
          serilogConfig = serilogConfig.MinimumLevel.Information();
          break;
        case AppLogLevel.Warning:
          serilogConfig = serilogConfig.MinimumLevel.Warning();
          break;
        case AppLogLevel.Error:
        case AppLogLevel.Critical:
          serilogConfig = serilogConfig.MinimumLevel.Error();
          break;
        default:
          throw new NotImplementedException($"Unknown {nameof(AppLogLevel)}.{config.LogLevel}");
      }

      if (config.LogSinks == null || config.LogSinks.Count() == 0)
      {
        serilogConfig = serilogConfig.WriteTo.Console();
      }
      else
      {
        foreach (var sink in config.LogSinks)
        {
          switch (sink.SinkType)
          {
            case LogSinkType.Console:
              serilogConfig = serilogConfig.WriteTo.Console();
              break;
            case LogSinkType.File:
              serilogConfig = serilogConfig.WriteTo.File(sink.Destination, rollingInterval: RollingInterval.Day);
              break;
            default:
              throw new NotImplementedException($"Unknown {nameof(LogSinkType)}.{sink.SinkType}");
          }
        }
      }

      return serilogConfig;
    }
  }
}
