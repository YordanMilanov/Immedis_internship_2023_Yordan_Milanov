using HCMS.Common.Structures;
using Newtonsoft.Json;
namespace HCMS.Common.JsonConverter
{
    public class PhoneConverter : JsonConverter<Phone>
    {
        public override Phone ReadJson(JsonReader reader, Type objectType, Phone existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string phoneNumber = (string)reader.Value! ?? "";
            return new Phone(phoneNumber);
        }

        public override void WriteJson(JsonWriter writer, Phone value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
