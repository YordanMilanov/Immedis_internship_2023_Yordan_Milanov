using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using HCMS.Common.Structures;

namespace HCMS.Common.JsonConverter
{
    public class LocationConverter : JsonConverter<LocationStruct>
    {
        public override LocationStruct ReadJson(JsonReader reader, Type objectType, LocationStruct existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            string? address = obj.Value<string>("address");
            string state = obj.Value<string>("state")!;
            string country = obj.Value<string>("country")!;
            return new LocationStruct(address, state, country);
        }

        public override void WriteJson(JsonWriter writer, LocationStruct value, JsonSerializer serializer)
        {
            JObject obj = new JObject();
            obj.Add("address", value.GetAddress());
            obj.Add("state", value.GetState());
            obj.Add("country", value.GetCountry());
            obj.WriteTo(writer);
        }
    }
}
