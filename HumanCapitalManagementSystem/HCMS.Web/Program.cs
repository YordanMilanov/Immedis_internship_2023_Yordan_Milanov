using Microsoft.AspNetCore.Authentication.Cookies;
using HCMS.Web.Extensions;

namespace HCMS.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            //services
            builder.Services.AddServices();

            //http client
            builder.Services.AddHttpClient("WebApi", client =>
            {
                client.BaseAddress = new Uri("https://localhost:9090/");
            });

            //Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
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