using Searchle.Common.Configuration;

namespace Searchle.Dictionary.Business.Configuration
{
  public class PlaintextSecret : ISecret
  {
    private ISecretRepository _secretRepository;
    private string _key;
    private string _value = string.Empty;
    private bool _fetchedDataFromRepository = false;

    public PlaintextSecret(ISecretRepository secretRepository, string key)
    {
      _secretRepository = secretRepository;
      _key = key;
    }

    public string Key { get { return _key; } }

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
      if (_fetchedDataFromRepository)
      {
        return _value;
      }

      _value = await _secretRepository.FetchSecretValueAsync(_key);
      _fetchedDataFromRepository = true;
      return _value;
    }
  }
}
