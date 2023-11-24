using HCMS.Common.JsonConverter;
using Newtonsoft.Json;

public static class JsonSerializerSettingsProvider
{
    public static JsonSerializerSettings GetCustomSettings()
    {
        var settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter>
            {
                new NameConverter(),
                new EmailConverter(),
                new PasswordConverter(),
                new RoleConverter(),
                new PhotoConverter(),
                new PhoneConverter(),
                new LocationConverter(),
                new DescriptionConverter(),
            }
        };

        return settings;
    }
}
