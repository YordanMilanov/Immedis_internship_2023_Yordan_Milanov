using HCMS.Web.WebServices;
using HCMS.Web.WebServices.Interfaces;
using Newtonsoft.Json;

namespace HCMS.Web.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyWebService, CompanyWebService>();
        }
    }
}
