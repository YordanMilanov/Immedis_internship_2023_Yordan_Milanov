using HCMS.Common.Structures;
using Newtonsoft.Json;

namespace HCMS.Common.JsonConverter
{
    public class SalaryConverter : JsonConverter<Salary>
    {
        // Reads and casts it to Salary
        public override Salary ReadJson(JsonReader reader, Type objectType, Salary existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Integer)
            {
                decimal salary = Convert.ToDecimal(reader.Value!);
                return new Salary(salary);
            }
            return new Salary(0);
        }
        public override void WriteJson(JsonWriter writer, Salary salary, JsonSerializer serializer)
        {
            writer.WriteValue(salary.GetValue());
        }
    }
}
