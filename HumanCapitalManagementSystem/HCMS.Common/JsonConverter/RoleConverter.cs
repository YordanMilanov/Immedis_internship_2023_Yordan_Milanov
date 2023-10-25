using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Common.JsonConverter
{
    public class RoleConverter : JsonConverter<RoleStruct>
    {
        public override RoleStruct ReadJson(JsonReader reader, Type objectType, RoleStruct existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var roleName = (string)reader.Value!;
                return new RoleStruct(roleName!);
            }
            return new RoleStruct("");
        }

        public override void WriteJson(JsonWriter writer, RoleStruct value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
