using System;

namespace Searchle.Common.Logging
{
  public class AppLoggingConfig
  {
    public AppLogLevel LogLevel = AppLogLevel.None;
    public IEnumerable<LogSink> LogSinks { get; set; } = new LogSink[0];
  }
}
