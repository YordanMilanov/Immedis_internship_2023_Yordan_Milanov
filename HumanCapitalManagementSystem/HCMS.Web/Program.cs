using System.Security.Cryptography.X509Certificates;
using HCMS.Common;
using HCMS.Data;
using HCMS.Services;
using HCMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using HCMS.Data.Models;
using HCMS.Repository;
using HCMS.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace HCMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = 
                builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services
                .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //TODO: Register Services!
            
            //services and repositories should also be added!
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
            builder.Services.AddScoped<ICompanyService, CompanyService>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            //Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); //session timeout if not used.
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.AccessDeniedPath = "/User/AccessDenied"; // Set your access denied path
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Set the cookie expiration time

                    options.LoginPath = "/User/Login";
                    options.LogoutPath = "/User/Logout";
                });

            //Authorization
            //builder.Services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Agent", policy => policy.RequireRole(RoleConstants.AGENT));
            //    options.AddPolicy("Employee", policy => policy.RequireRole(RoleConstants.EMPLOYEE));
            //    options.AddPolicy("Admin", policy => policy.RequireRole(RoleConstants.ADMIN));
            //});

            //Controllers
            builder.Services.AddControllersWithViews();



            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //!app.Environment.IsDevelopment()
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();


        }
    }
}