using AutoMapper;

public static class AutoMapperHelper
{
    public static IMapper InitializeMapper()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            var assembly = typeof(HCMS.Services.AutoMapperProfiles.AdvertMappingProfile).Assembly;

            var profileTypes = assembly.GetTypes()
                .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract);

            foreach (var profileType in profileTypes)
            {
                cfg.AddProfile((Profile)Activator.CreateInstance(profileType)!);
            }
        });
        return configuration.CreateMapper();
    }
}
