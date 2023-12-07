using HCMS.Services.Implementation;
using HCMS.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HCMS.UnitTest.Services")]
namespace HCMS.Services
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService,UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEducationService, EducationService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IWorkRecordService, WorkRecordService>();
            services.AddScoped<IRecommendationService, RecommendationService>();
            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<IApplicationService, ApplicationService>();
        }
    }
}
