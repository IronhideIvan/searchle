namespace Searchle.Configuration.Interfaces
{
  public interface ISecretRepository
  {
    string FetchSecretValue(string valueKey);
    Task<string> FetchSecretValueAsync(string valueKey);
  }
}
