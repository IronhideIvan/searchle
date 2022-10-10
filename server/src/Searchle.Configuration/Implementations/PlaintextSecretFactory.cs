using Searchle.Configuration.Interfaces;

namespace Searchle.Configuration.Implementations
{
  public class PlaintextSecretFactory : ISecretFactory
  {
    private ISecretRepository _secretRepository;

    public PlaintextSecretFactory(ISecretRepository secretRepository)
    {
      _secretRepository = secretRepository;
    }

    public ISecret Create(string key)
    {
      return new PlaintextSecret(_secretRepository, key);
    }
  }
}
