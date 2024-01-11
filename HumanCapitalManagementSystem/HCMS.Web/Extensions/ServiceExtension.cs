using HCMS.Web.WebServices;
using HCMS.Web.WebServices.Interfaces;

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
