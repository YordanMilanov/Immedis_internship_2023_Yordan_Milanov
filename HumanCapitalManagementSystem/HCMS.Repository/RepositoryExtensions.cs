using HCMS.Repository.Interfaces;
using HCMS.Repository.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace HCMS.Repository
{
    public static class RepositoryExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IWorkRecordRepository, WorkRecordRepository>();
            services.AddScoped<IEducationRepository, EducationRepository>();
        }
    }
}
