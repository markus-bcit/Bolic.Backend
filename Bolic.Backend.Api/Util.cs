using System.Text.Json;
using System.Text.Json.Serialization;
namespace Bolic.Backend.Api;


public class NumberToStringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.Null => null,
            _ => JsonDocument.ParseValue(ref reader).RootElement.GetRawText()
        };
    }
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteStringValue(value);
}