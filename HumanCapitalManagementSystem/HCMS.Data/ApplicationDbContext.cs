﻿using HCMS.Common;
using HCMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("HCMS.UnitTests")]
namespace HCMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration? configuration;

        //for production
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        //for tests
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Advert> Adverts { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Education> Educations { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Recommendation> Recommendations { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserRole> UsersRoles { get; set; } = null!;
        public DbSet<UserClaim> UsersClaims { get; set; } = null!;
        public DbSet<WorkRecord> WorkRecords { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(ApplicationDbContext)) ??
                                      Assembly.GetExecutingAssembly();

            builder.ApplyConfigurationsFromAssembly(configAssembly);


            builder.Entity<Education>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired(false);
                entity.Property(e => e.University)
                    .IsRequired(false)
                    .HasMaxLength(DataModelConstants.Education.UniversityMaxLength);
                entity.Property(e => e.Degree)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Education.DegreeMaxLength);
                entity.Property(e => e.FieldOfEducation)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Education.FieldOfEducationMaxLength);
                entity.Property(e => e.Grade).IsRequired();

                entity.Property(e => e.EmployeeId).IsRequired();
                entity.HasOne(e => e.Employee)
                    .WithMany(e => e.Educations)
                    .HasForeignKey(e => e.EmployeeId);

                entity.Property(e => e.LocationId).IsRequired(false);
                entity.HasOne(e => e.Location)
                   .WithOne()
                   .HasForeignKey<Education>(e => e.LocationId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.CoverLetter);
                entity.Property(e => e.AddDate).IsRequired();

                //FKs
                entity.Property(e => e.AdvertId).IsRequired();
                entity.HasOne(e => e.Advert)
                    .WithMany()
                    .HasForeignKey(e => e.AdvertId);

                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.FromEmployeeId).IsRequired()
                    .HasPrincipalKey(e => e.Id);

                entity.HasIndex(e => new { e.FromEmployeeId, e.AdvertId })
                .IsUnique()
                .HasDatabaseName("UC_AdvertId_FromEmployeeId_Index");
            });
            builder.Entity<Advert>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Advert.PositionMaxLength);
                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Advert.DepartmentMaxLength);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Salary).IsRequired();
                entity.Property(e => e.AddDate).IsRequired();
                entity.Property(e => e.RemoteOption).IsRequired();

                //FKs
                entity.Property(e => e.CompanyId).IsRequired();
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId);

                entity.HasMany(e => e.Applications)
                     .WithOne(e => e.Advert)
                     .HasForeignKey(a => a.AdvertId);


            });
            builder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.VarcharDefaultLength);
                entity.Property(e => e.Description);
                entity.Property(e => e.IndustryField)
                    .HasMaxLength(DataModelConstants.Company.IndustryFieldMaxLength);
                entity.Property(e => e.LocationId);

                //FKS
                entity.HasOne(e => e.Location)
                    .WithOne()
                    .HasForeignKey<Company>(e => e.LocationId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Adverts)
                   .WithOne(a => a.Company)
                   .HasForeignKey(a => a.CompanyId);
            });
            builder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Employee.FirstNameMaxLength);
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Employee.LastNameMaxLength);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Employee.EmailMaxLength);
                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Employee.PhoneNumberMaxLength);
                entity.Property(e => e.PhotoUrl);
                entity.Property(e => e.DateOfBirth).IsRequired();
                entity.Property(e => e.AddDate).IsRequired();

                entity.Property(e => e.CompanyId);
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .IsRequired(false);

                entity.Property(e => e.UserId);
                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .IsRequired(false);

                entity.Property(e => e.LocationId);
                entity.HasOne(e => e.Location)
                    .WithMany()
                    .HasForeignKey(e => e.LocationId)
                    .IsRequired(false);

                entity.HasMany(e => e.WorkRecords)
            .WithOne(e => e.Employee);

                entity.HasMany(e => e.Educations)
                    .WithOne(e => e.Employee);

                entity.HasMany(e => e.Applications)
                    .WithOne(e => e.Employee)
                    .HasForeignKey(e => e.FromEmployeeId);
            });
            builder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Address)
                    .HasMaxLength(DataModelConstants.Location.AddressMaxLength);
                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Location.StateMaxLength);
                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Location.CountryMaxLength);
                entity.Property(e => e.OwnerId)
                    .IsRequired();
                entity.Property(e => e.OwnerType)
                    .IsRequired();
            });
            builder.Entity<Recommendation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Description)
                    .IsRequired();
                entity.Property(e => e.RecommendDate)
                    .IsRequired();

                entity.Property(e => e.ForEmployeeId)
                    .IsRequired();
                entity.HasOne(e => e.ForEmployee)
                    .WithMany()
                    .HasForeignKey(e => e.ForEmployeeId)
                    .IsRequired();

                entity.Property(e => e.FromEmployeeId)
                   .IsRequired();
                entity.HasOne(e => e.FromEmployee)
                    .WithMany()
                    .HasForeignKey(e => e.FromEmployeeId)
                    .IsRequired();

                entity.Property(e => e.ToCompanyId)
                    .IsRequired();
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.ToCompanyId)
                    .IsRequired();
            });
            builder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Role.RoleNameMaxLength);
                entity.Property(e => e.Description)
                    .HasMaxLength(DataModelConstants.VarcharDefaultLength);
            });
            builder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Users_Id");
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.User.UsernameMaxLength)
                    .HasColumnName("Username");
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.User.PasswordMaxLength)
                    .HasColumnName("Password");
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.User.EmailMaxLength)
                    .HasColumnName("Email");
                entity.Property(e => e.RegisterDate)
                    .IsRequired()
                    .HasColumnName("RegisterDate");
                //Constraints
                entity.HasIndex(e => e.Username, "UC_Users_Username_Unique").IsUnique();
                entity.HasIndex(e => e.Email, "UC_Users_Email_Unique").IsUnique();

                //FKs
                entity.HasMany(e => e.UsersRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId);
                entity.HasMany(e => e.UserClaims)
                    .WithOne(uc => uc.User)
                    .HasForeignKey(uc => uc.UserId);
            });
            builder.Entity<UserClaim>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.UserId)
                    .IsRequired();
                entity.Property(e => e.ClaimType);
                entity.Property(e => e.ClaimValue);

                entity.HasOne(e => e.User)
                    .WithMany(u => u.UserClaims)
                    .HasForeignKey(e => e.UserId);
            });
            builder.Entity<WorkRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Position)
                    .IsRequired();

                entity.Property(e => e.Department);

                entity.Property(e => e.Salary)
                    .IsRequired();

                entity.Property(e => e.StartDate)
                    .IsRequired();

                entity.Property(e => e.EndDate);

                entity.Property(e => e.AddDate)
                .IsRequired();

                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .IsRequired();

                entity.Property(e => e.CompanyId)
                   .IsRequired();
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId)
                    .IsRequired();
            });
            //USER-ROLES
            // Configure the many-to-many relationship
            builder.Entity<UserRole>(entity =>
            {
                // Configure the primary key
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UsersRoles)
                    .HasForeignKey(ur => ur.UserId);

                entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UsersRoles)
                .HasForeignKey(ur => ur.RoleId);
            });

            base.OnModelCreating(builder);


        }

        public static DbContextOptions<ApplicationDbContext> GetApplicationDbOptions()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(ConnectionConfiguration.ConnectionString);
            return builder.Options;
        }

    }
}
