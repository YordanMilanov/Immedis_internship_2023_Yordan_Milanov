using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HCMS.Common;
using Microsoft.EntityFrameworkCore;
using HCMS.Data.Models;

namespace HCMS.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        //TODO: Add all DbSets
        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Education> Educations { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Recommendation> Recommendations { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<WorkRecord> WorkRecords { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(ConnectionConfiguration.ConnectionString);
            }
        }

        //TODO: Define entity relations FKs
        protected override void OnModelCreating(ModelBuilder builder)
        {
            Assembly configAssembly = Assembly.GetAssembly(typeof(ApplicationDbContext)) ??
                                      Assembly.GetExecutingAssembly();

            builder.ApplyConfigurationsFromAssembly(configAssembly);

            base.OnModelCreating(builder);
        }
    }
}
