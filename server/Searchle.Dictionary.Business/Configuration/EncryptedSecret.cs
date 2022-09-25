using EncryptionUtility.Crypto;
using System;
using Searchle.Common.Configuration;

namespace Searchle.Dictionary.Business.Configuration
{
  public class EncryptedSecret : ISecret
  {
    private ISecretRepository _secretRepository;
    private string _key = string.Empty;
    private string _passwordKey = string.Empty;
    private string _plaintextValue = string.Empty;
    private bool _isValueFetched = false;

    public string Key => _key;

    public EncryptedSecret(ISecretRepository secretRepository, string key, string pwKey)
    {
      _secretRepository = secretRepository;
      _key = key;
      _passwordKey = pwKey;
    }

    public string GetValue()
    {
      var task = Task.Run(async () =>
      {
        return await GetValueAsync();
      });

      if (!task.IsCompleted)
      {
        task.Wait();
      }

      return task.Result;
    }

    public async Task<string> GetValueAsync()
    {
      if (_isValueFetched)
      {
        return _plaintextValue;
      }

      var encryptedValue = await _secretRepository.FetchSecretValueAsync(_key);
      var passwordValue = await _secretRepository.FetchSecretValueAsync(_passwordKey);

      var encryptedValueBytes = Convert.FromBase64String(encryptedValue);
      var passwordBytes = Convert.FromBase64String(encryptedValue);

      var encryptionUtil = new AesEncryption();
      var rawValueBytes = await encryptionUtil.DecryptAsync(encryptedValueBytes, passwordBytes);
      _plaintextValue = System.Text.Encoding.UTF8.GetString(rawValueBytes);
      _isValueFetched = true;

      return _plaintextValue;
    }
  }
}
