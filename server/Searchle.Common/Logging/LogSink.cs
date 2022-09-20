using System;

namespace Searchle.Common.Logging
{
  public class LogSink
  {
    public LogSinkType SinkType { get; set; }
    public string Destination { get; set; } = string.Empty;
  }
}
