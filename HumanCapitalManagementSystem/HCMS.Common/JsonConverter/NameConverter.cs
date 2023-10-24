using HCMS.Common.Structures;
using Newtonsoft.Json;
using System;


namespace HCMS.Common.JsonConverter
{
    
    public class NameConverter : JsonConverter<Name>
    {
        //reads and cast it to Name
        public override Name ReadJson(JsonReader reader, Type objectType, Name existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string name = (string)reader.Value;
                if (name == null)
                {
                    //default value if name is null
                    return new Name("Not defined");
                }
                //else parse it
                return new Name(name);
            }

            //default value
            return new Name("Not defined");
        }

        //cast from Name to string
        public override void WriteJson(JsonWriter writer, Name value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
