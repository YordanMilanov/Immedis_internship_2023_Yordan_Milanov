using HCMS.Common.Structures;
using Newtonsoft.Json;


namespace HCMS.Common.JsonConverter
{
    public class PasswordConverter : JsonConverter<Password>
    {
        public override Password ReadJson(JsonReader reader, Type objectType, Password existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string password = (string)reader.Value;
                if (password != null)
                {
                    return new Password(password);
                }
            }
            return new Password("");
        }

        public override void WriteJson(JsonWriter writer, Password value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
