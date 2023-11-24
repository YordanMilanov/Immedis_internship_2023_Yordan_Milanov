using HCMS.Common;
using HCMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HCMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Education> Educations { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Data.Models.Location> Locations { get; set; } = null!;
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
                    .UseSqlServer(ConnectionConfiguration.ConnectionString);
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
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired(false);
                entity.Property(e => e.University)
                    .IsRequired()
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
                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Application.PositionMaxLength);
                entity.Property(e => e.Department)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Application.DepartmentMaxLength);
                entity.Property(e => e.CoverLetter).IsRequired();
                entity.Property(e => e.AddDate).IsRequired();
                entity.Property(e => e.ToCompanyId).IsRequired();

                //FKs
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.ToCompanyId);

                entity.Property(e => e.FromEmployeeId).IsRequired();

                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.FromEmployeeId);
            });
            builder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.VarcharDefaultLength);
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.IndustryField)
                    .IsRequired()
                    .HasMaxLength(DataModelConstants.Company.IndustryFieldMaxLength);
                entity.Property(e => e.LocationId);

                entity.HasOne(e => e.Location)
                    .WithOne()
                    .HasForeignKey<Company>(e => e.LocationId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);
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
                entity.Property(e => e.AddDate);
               
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

                entity.HasMany(e => e.Educations)
                    .WithOne(e => e.Employee);
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
                    .IsRequired(false);

                entity.Property(e => e.FromEmployeeId)
                   .IsRequired();
                entity.HasOne(e => e.FromEmployee)
                    .WithMany()
                    .HasForeignKey(e => e.FromEmployeeId)
                    .IsRequired(false);

                entity.Property(e => e.ToCompanyId)
                    .IsRequired();
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.ToCompanyId)
                    .IsRequired(false);
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
                entity.HasKey(e => e.Id);
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
    }
}
