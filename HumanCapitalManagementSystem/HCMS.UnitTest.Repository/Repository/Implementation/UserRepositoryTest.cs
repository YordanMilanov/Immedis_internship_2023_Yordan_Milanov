using AutoFixture;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Repository.Implementation;
using HCMS.Repository.Interfaces;
using HCMS.Tests.AutoFixtureCustomization.User;
using Microsoft.EntityFrameworkCore;

namespace HCMS.Tests.Repository.Implementation
{
    public class UserRepositoryTest
    {
        private ApplicationDbContext dbContext;
        private IUserRepository userRepository;
        private IFixture fixture;
        private User user;
       
       
        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InMemoryDb")
                .Options;
            dbContext = new ApplicationDbContext(options);
            userRepository = new UserRepository(dbContext);

            user = new User()
            {
                Username = "Username",
                Email = "Email@mail.com",
                Password = "Password",
                UsersRoles = new List<UserRole>(),
                UserClaims = new List<UserClaim>()
            };
        }

        [TearDown]
        public void Teardown()
        {
            dbContext.Dispose();
        }

        [Test]
        public void UserRepository_Throws_If_User_Does_Not_Have_Email()
        {
            // Arrange
            user.Email = null;

            // Act
            TestDelegate act = () => userRepository.RegisterUser(user);

            // Assert
            Assert.That(act, Throws.TypeOf<DbUpdateException>());

            Assert.IsTrue(dbContext.Users.Contains(user));
        }
    }
}
