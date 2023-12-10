using HCMS.Data;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Tests.Unit
{
    public static class InMemoryDbContextOptions
    {
        public static DbContextOptions<ApplicationDbContext> GetApplicationDbOptions()
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase("InMemoryDb");
            return builder.Options;
        }
    }
}
