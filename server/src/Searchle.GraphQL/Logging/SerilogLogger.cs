using Searchle.Common.Exceptions;
using Searchle.Common.Logging;

namespace Searchle.GraphQL.Logging
{
  public class SerilogLogger<T> : IAppLogger<T>
  {
    private AppLoggingConfig _config;
    private Serilog.ILogger _logger;

    public SerilogLogger(AppLoggingConfig config, Serilog.ILogger logger)
    {
      _config = config;
      _logger = logger;
    }

    public void Debug(string messageTemplate, params object[] propertyValues)
    {
      Debug(messageTemplate, null, propertyValues);
    }

    public void Debug(string messageTemplate, Exception? ex, params object[] propertyValues)
    {
      WriteLog(messageTemplate, AppLogLevel.Debug, ex, propertyValues);
    }

    public void Information(string messageTemplate, params object[] propertyValues)
    {
      Information(messageTemplate, null, propertyValues);
    }

    public void Information(string messageTemplate, Exception? ex, params object[] propertyValues)
    {
      WriteLog(messageTemplate, AppLogLevel.Information, ex, propertyValues);
    }

    public void Warning(string messageTemplate, params object[] propertyValues)
    {
      Warning(messageTemplate, null, propertyValues);
    }

    public void Warning(string messageTemplate, Exception? ex, params object[] propertyValues)
    {
      WriteLog(messageTemplate, AppLogLevel.Warning, ex, propertyValues);
    }

    public void Error(string messageTemplate, params object[] propertyValues)
    {
      Error(messageTemplate, null, propertyValues);
    }

    public void Error(string messageTemplate, Exception? ex, params object[] propertyValues)
    {
      WriteLog(messageTemplate, AppLogLevel.Error, ex, propertyValues);
    }

    public void Critical(string messageTemplate, params object[] propertyValues)
    {
      Critical(messageTemplate, null, propertyValues);
    }

    public void Critical(string messageTemplate, Exception? ex, params object[] propertyValues)
    {
      WriteLog(messageTemplate, AppLogLevel.Critical, ex, propertyValues);
    }

    private void WriteLog(string messageTemplate, AppLogLevel logAsLevel, Exception? ex, params object[] propertyValues)
    {
      if (_config.LogLevel > logAsLevel)
      {
        return;
      }

      // Wrap the message with additional details
      string logMessage = "[{LogSource}] " + messageTemplate;
      var logProp = new object[] { typeof(T).Name };

      // Prepend the value of our to property to the props object
      var newProps = propertyValues;
      if (propertyValues == null || propertyValues.Length == 0)
      {
        newProps = logProp;
      }
      else
      {
        newProps = new object[logProp.Length + propertyValues.Length];
        logProp.CopyTo(newProps, 0);
        propertyValues.CopyTo(newProps, logProp.Length);
      }

      try
      {
        switch (logAsLevel)
        {
          case AppLogLevel.Debug:
            _logger.Debug(ex, logMessage, newProps);
            break;

          case AppLogLevel.Information:
            _logger.Information(ex, logMessage, newProps);
            break;

          case AppLogLevel.Warning:
            _logger.Warning(ex, logMessage, newProps);
            break;

          case AppLogLevel.Error:
            _logger.Error(ex, logMessage, newProps);
            break;
          case AppLogLevel.Critical:
            _logger.Fatal(ex, logMessage, newProps);
            break;

          default:
            break;
        }
      }
      catch (Exception ex2)
      {
        throw new SearchleLoggerException($"Exception encountered while attempting to log message '{logMessage}'", ex2);
      }
    }
  }
}
