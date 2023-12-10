using HCMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace HCMS.Tests.Unit.Repository
{
    public abstract class BaseRepositoryTest
    {

        protected ApplicationDbContext dbContext;

        [SetUp]
        public void DatabaseSetup()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase("InMemoryDb");

            dbContext = new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
