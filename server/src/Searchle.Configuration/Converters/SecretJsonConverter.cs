using Newtonsoft.Json;
using Searchle.Configuration.Interfaces;

namespace Searchle.Configuration.Converters
{
  public class SecretJsonConverter : Newtonsoft.Json.JsonConverter<ISecret>
  {
    private ISecretFactory _secretFactory;

    public SecretJsonConverter(ISecretFactory secretFactory)
    {
      _secretFactory = secretFactory;
    }

    public override ISecret ReadJson(
      JsonReader reader,
      Type objectType,
      ISecret? existingValue,
      bool hasExistingValue,
      JsonSerializer serializer
    )
    {
      var strVal = reader.Value?.ToString();
      if (reader.TokenType != JsonToken.String || strVal == null)
      {
        throw new JsonSerializationException($"Value for {nameof(ISecret)} must be a non-null string. Path: {reader.Path}");
      }

      return _secretFactory.Create(strVal);
    }

    public override void WriteJson(JsonWriter writer, ISecret? value, JsonSerializer serializer)
    {
      throw new NotImplementedException();
    }
  }
}
