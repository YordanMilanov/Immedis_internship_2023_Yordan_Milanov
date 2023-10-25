using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Common.JsonConverter
{
    public class EmailConverter : JsonConverter<Email>
    {
        public override Email ReadJson(JsonReader reader, Type objectType, Email existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string email = (string)reader.Value;
                if (email == null)
                {
                    // Default value if the value is null
                    return new Email("");
                }
                // Parse the string and create an Email instance
                return new Email(email);
            }

            // Default value
            return new Email("");
        }

        public override void WriteJson(JsonWriter writer, Email value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
