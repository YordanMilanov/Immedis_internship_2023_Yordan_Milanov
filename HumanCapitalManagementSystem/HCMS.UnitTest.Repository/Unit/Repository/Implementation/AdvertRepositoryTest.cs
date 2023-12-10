using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Implementation;
using HCMS.Repository.Interfaces;
using HCMS.Tests.Unit.Repository;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Tests.Repository.Implementation
{
    internal class AdvertRepositoryTest : BaseRepositoryTest
    {
        private Advert advert;
        private AdvertRepository advertRepository;

        [SetUp]
        public void Setup()
        {
            //Arrange
            advertRepository = new AdvertRepository(dbContext);

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

            // Act
            advert.Position = null!;
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
            // Act
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
            // Act
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
            //Act
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

        //Default parameters test
        [Test]
        public async Task GetCurrentPageAsync_Default_Parameters_Works_Test()
        {
            // Arrange
            var advertRepository = new AdvertRepository(dbContext);
            QueryParameterClass queryParameterClass = new QueryParameterClass();
            queryParameterClass.ItemsPerPage = 10;
            queryParameterClass.OrderPageEnum = Common.OrderPageEnum.Newest;
            queryParameterClass.SearchString = null;
            bool? remoteOption = null;

            Guid companyId = await dbContext.Adverts
                .OrderBy(a => a.Id)
                .Select(a => a.CompanyId)
                .FirstOrDefaultAsync();

            await advertRepository.GetCurrentPageAsync(queryParameterClass, remoteOption, companyId.ToString());

        }
    }
}
