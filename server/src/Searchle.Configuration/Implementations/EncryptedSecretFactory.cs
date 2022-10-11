using Searchle.Configuration.Interfaces;
using Searchle.Configuration.Models;

namespace Searchle.Configuration.Implementations
{
  public class EncryptedSecretFactory : ISecretFactory
  {
    private ISecretRepository _secretRepository;
    private SecretConfiguration _secretConfig;

    public EncryptedSecretFactory(ISecretRepository secretRepository, SecretConfiguration config)
    {
      _secretRepository = secretRepository;
      _secretConfig = config;
    }

    public ISecret Create(string key)
    {
      return new EncryptedSecret(_secretRepository, key, _secretConfig.RootKey);
    }
  }
}
