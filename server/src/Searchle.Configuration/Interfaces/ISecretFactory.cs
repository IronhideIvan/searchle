namespace Searchle.Configuration.Interfaces
{
  public interface ISecretFactory
  {
    ISecret Create(string key);
  }
}
