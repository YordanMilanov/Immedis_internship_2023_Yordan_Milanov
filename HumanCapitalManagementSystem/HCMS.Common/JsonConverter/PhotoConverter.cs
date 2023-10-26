namespace HCMS.Common.JsonConverter
{
    using HCMS.Common.Structures;
    using Newtonsoft.Json;
    using System;

    public class PhotoConverter : JsonConverter<Photo>
    {
        public override Photo ReadJson(JsonReader reader, Type objectType, Photo existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new Photo(null);
            }

            if (reader.TokenType == JsonToken.String)
            {
                string value = (string)reader.Value! ?? "";
                return new Photo(value);
            }

            throw new JsonReaderException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, Photo value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
