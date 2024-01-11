using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Common.JsonConverter
{

    public class NameConverter : JsonConverter<Name>
    {
        //reads and cast it to Name
        public override Name ReadJson(JsonReader reader, Type objectType, Name existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string name = (string)reader.Value!;
                if (name != null)
                {
                    return new Name(name);
                }
            }
            return new Name("");
        }

        //cast from Name to string
        public override void WriteJson(JsonWriter writer, Name value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
