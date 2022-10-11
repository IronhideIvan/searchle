namespace Searchle.Configuration.Models
{
  public class SecretConfiguration
  {
    public SecretSource Source { get; set; }
    public string RootKey { get; set; } = string.Empty;
    public bool Encrypted { get; set; }
  }
}
