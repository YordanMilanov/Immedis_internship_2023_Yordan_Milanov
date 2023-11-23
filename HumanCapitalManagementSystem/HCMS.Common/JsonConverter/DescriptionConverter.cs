using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Common.JsonConverter
{
    public class DescriptionConverter : JsonConverter<Description>
    {
        //reads and cast it to Description
        public override Description ReadJson(JsonReader reader, Type objectType, Description existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string name = (string)reader.Value!;
                if (name != null)
                {
                    return new Description(name);
                }
            }
            return new Description("");
        }

        //cast from Description to string
        public override void WriteJson(JsonWriter writer, Description value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
