
using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Common.JsonConverter
{
    public class DepartmentConverter : JsonConverter<Department>
    {
        //reads and cast it to Description
        public override Department ReadJson(JsonReader reader, Type objectType, Department existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string department = (string)reader.Value!;
                if (department != null)
                {
                    return new Department(department);
                }
            }
            return new Department("");
        }

        //cast from Description to string
        public override void WriteJson(JsonWriter writer, Department department, JsonSerializer serializer)
        {
            writer.WriteValue(department.ToString());
        }
    }
}
