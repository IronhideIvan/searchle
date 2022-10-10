namespace Searchle.Configuration.Interfaces
{
  public interface ISecret
  {
    string Key { get; }
    string GetValue();
    Task<string> GetValueAsync();
  }
}
