using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Implementation;
using Microsoft.EntityFrameworkCore;

namespace HCMS.UnitTest.Repository.Implementation
{
    internal class AdvertRepositoryTest
    {
        private ApplicationDbContext dbContext;
        private Advert advert;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;

            dbContext = new ApplicationDbContext(options);

            //populate the advert correctly
            advert = new Advert();
            advert.Position = "Position";
            advert.Department = "Department";
            advert.Description = "Testing description to check if it works correctly!";
            advert.RemoteOption = false;
            advert.Salary = 1000;
            advert.AddDate = DateTime.Now;
            advert.CompanyId = Guid.NewGuid();
        }

        [TearDown]
        public void Teardown()
        {
            dbContext.Dispose();
        }

        [Test]
        public async Task AddAsync_Should_Not_Add_Missing_Position()
        {
            // Arrange
            var advertRepository = new AdvertRepository(dbContext);
            advert.Position = null!;
            // Act
            // Assert
            Assert.ThrowsAsync<DbUpdateException>(async () => await advertRepository.AddAsync(advert));
            Assert.IsFalse(dbContext.Adverts.Contains(advert));
            Assert.IsNull(await dbContext.Adverts.FirstOrDefaultAsync(a => a.Id == advert.Id));
        }

        [Test]
        public async Task AddAsync_Should_Not_Add_Missing_Department()
        {
            // Arrange
            var advertRepository = new AdvertRepository(dbContext);
            advert.Department = null!;
            // Assert
            Assert.ThrowsAsync<DbUpdateException>(async () => await advertRepository.AddAsync(advert));
            Assert.IsFalse(dbContext.Adverts.Contains(advert));
            Assert.IsNull(await dbContext.Adverts.FirstOrDefaultAsync(a => a.Id == advert.Id));
        }

        [Test]
        public async Task AddAsync_Should_Not_Add_Missing_CompanyId()
        {
            // Arrange
            var advertRepository = new AdvertRepository(dbContext);
            advert.CompanyId = Guid.Empty;
            // Assert
            Assert.ThrowsAsync<Exception>(async () => await advertRepository.AddAsync(advert));
            Assert.IsFalse(dbContext.Adverts.Contains(advert));
            Assert.IsNull(await dbContext.Adverts.FirstOrDefaultAsync(a => a.Id == advert.Id));
        }

        [Test]
        public async Task AddAsync_Should_Not_Add_Missing_Description()
        {
            // Arrange
            var advertRepository = new AdvertRepository(dbContext);
            advert.Description = null!;
            // Assert
            Assert.ThrowsAsync<DbUpdateException>(async () => await advertRepository.AddAsync(advert));
            Assert.IsFalse(dbContext.Adverts.Contains(advert));
            Assert.IsNull(await dbContext.Adverts.FirstOrDefaultAsync(a => a.Id == advert.Id));
        }

        //Happy Path
        [Test]
        public async Task AddAsync_Should_Add_Advert_To_Database()
        {
            // Arrange
            var advertRepository = new AdvertRepository(dbContext);
            // Act
            await advertRepository.AddAsync(advert);
            // Assert
            Assert.IsTrue(dbContext.Adverts.Contains(advert));
            Assert.That(advert, Is.EqualTo(await dbContext.Adverts.FirstOrDefaultAsync(a => a.Id == advert.Id)));
        }
    }
}
