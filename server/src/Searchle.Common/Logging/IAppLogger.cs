namespace Searchle.Common.Logging;

public interface IAppLogger<T>
{
  void Debug(string messageTemplate, params object[] propertyValues);
  void Debug(string messageTemplate, Exception? ex, params object[] propertyValues);
  void Information(string messageTemplate, params object[] propertyValues);
  void Information(string messageTemplate, Exception? ex, params object[] propertyValues);
  void Warning(string messageTemplate, params object[] propertyValues);
  void Warning(string messageTemplate, Exception? ex, params object[] propertyValues);
  void Error(string messageTemplate, params object[] propertyValues);
  void Error(string messageTemplate, Exception? ex, params object[] propertyValues);
  void Critical(string messageTemplate, params object[] propertyValues);
  void Critical(string messageTemplate, Exception? ex, params object[] propertyValues);
}