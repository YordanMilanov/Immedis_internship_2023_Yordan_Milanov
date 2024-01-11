using CloudinaryDotNet;
using HCMS.Data;
using HCMS.Repository;
using HCMS.Services;
using HCMS.Services.AutoMapperProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Add configuration file
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//Add ApplicationDbContext
string connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//Cloudinary for pictures 
CloudinarySettings cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
Account account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
Cloudinary cloudinary = new Cloudinary(account);
builder.Services.AddSingleton(cloudinary);

builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));


builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddRepositories();

builder.Services.AddAutoMapper(typeof(EmployeeMappingProfile).Assembly);

//Add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),
        ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
