using Searchle.Common.Configuration;

namespace Searchle.Dictionary.Business.Configuration
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
