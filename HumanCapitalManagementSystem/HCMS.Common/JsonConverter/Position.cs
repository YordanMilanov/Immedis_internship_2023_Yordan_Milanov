using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Common.JsonConverter
{
    public class PositionConverter : JsonConverter<Position>
    {
        //reads and cast it to Description
        public override Position ReadJson(JsonReader reader, Type objectType, Position existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string position = (string)reader.Value!;
                if (position != null)
                {
                    return new Position(position);
                }
            }
            return new Position("");
        }

        //cast from Description to string
        public override void WriteJson(JsonWriter writer, Position position, JsonSerializer serializer)
        {
            writer.WriteValue(position.ToString());
        }
    }
}

