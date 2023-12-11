using AutoFixture;
using HCMS.Data;
using HCMS.Data.Models;
using HCMS.Data.Models.QueryPageGenerics;
using HCMS.Repository.Implementation;
using HCMS.Repository.Interfaces;
using HCMS.Tests.Unit.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HCMS.Tests.Repository.Implementation
{
    public class UserRepositoryTest : BaseRepositoryTest
    {
        private IUserRepository userRepository;
        private User user;
       
       
        [SetUp]
        public void Setup()
        {
            userRepository = new UserRepository(dbContext);

            user = new User()
            {
                Username = "Username",
                Email = "Email@mail.com",
                Password = "Password",
                RegisterDate = DateTime.Now,
                UsersRoles = new List<UserRole>(),
                UserClaims = new List<UserClaim>()
            };
        }

        [TearDown]
        public void Teardown()
        {
            dbContext.Dispose();
        }

        //RegisterUserAsync

        //Happy Path
        [Test]
        public async Task RegisterUserAsync_Should_Add_Successfully()
        {
            // Act
            // Assert
            Assert.DoesNotThrowAsync(async () => await userRepository.RegisterUserAsync(user));

            Assert.IsTrue(dbContext.Users.Contains(user));
            Assert.NotNull(await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id));
        }

        [Test]
        public async Task RegisterUserAsync_Should_Not_Add_With_Missing_Email()
        {
            //Arrange
            user.Email = null!;

            //act assert
            await AssertNotRegistered(user);
        }
        [Test]
        public async Task RegisterUserAsync_Should_Not_Add_With_Missing_Password()
        {
            //Arrange
            user.Password = null!;
            //act assert
            await AssertNotRegistered(user);
        }

        [Test]
        public async Task RegisterUserAsync_Should_Not_Add_With_Missing_Username()
        {
            //Arrange
            user.Username = null!;
            //act assert
            await AssertNotRegistered(user);
        }

        private async Task AssertNotRegistered(User user)
        {
            Assert.ThrowsAsync<DbUpdateException>(async () => await userRepository.RegisterUserAsync(user));
            Assert.IsFalse(dbContext.Users.Contains(user));
            Assert.IsNull(await dbContext.Adverts.FirstOrDefaultAsync(u => u.Id == user.Id));
        }

        [Test]
        public async Task RegisterUserAsync_Should_Not_Add_If_User_Id_Exists()
        {
            //Arrange
            await userRepository.RegisterUserAsync(user);

            //Assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.ThrowsAsync<ArgumentException>(async () => await userRepository.RegisterUserAsync(user));
        }

        [Test]
        public async Task RegisterUserAsync_Should_Not_Add_If_User_Username_Exists()
        {
            //Arrange
            User user2 = new User()
            {
                Username = "Username",
                Email = "NotTheSameEmail@mail.com",
                Password = "Password",
            };

            await userRepository.RegisterUserAsync(user);

            //Assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.ThrowsAsync<ConstraintException>(async () => await userRepository.RegisterUserAsync(user2));
        }

        [Test]
        public async Task RegisterUserAsync_Should_Not_Add_If_User_Email_Exists()
        {
            //Arrange
            User user2 = new User()
            {
                Username = "UsernameNotTheSame",
                Email = "Email@mail.com",
                Password = "Password",
            };

            await userRepository.RegisterUserAsync(user);

            //Assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.ThrowsAsync<ConstraintException>(async () => await userRepository.RegisterUserAsync(user2));
        }

        //UserExistsByUsernameAsync

        [Test]
        public async Task UserExistsByUsernameAsync_Should_Return_True_If_exists()
        {
            //arrange
            await userRepository.RegisterUserAsync(user);

            //assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.IsTrue(await userRepository.UserExistsByUsernameAsync(user.Username));
        }

        [Test]
        public async Task UserExistsByUsernameAsync_Should_Return_False_If_Not_Exists()
        {
            //arrange
            await userRepository.RegisterUserAsync(user);

            //assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.IsFalse(await userRepository.UserExistsByUsernameAsync("NotExistingName"));
        }

        //UserExistsByEmailAsync
        [Test]
        public async Task UserExistsByEmailAsync_Should_Return_True_If_exists()
        {
            //arrange
            await userRepository.RegisterUserAsync(user);

            
            //assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.IsTrue(await userRepository.UserExistsByEmailAsync(user.Email));
        }

        [Test]
        public async Task UserExistsByEmailAsync_Should_Return_False_If_Not_Exists()
        {
            //arrange
            await userRepository.RegisterUserAsync(user);

            //assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.IsFalse(await userRepository.UserExistsByEmailAsync("NotExistingEmail"));
        }

        //UserByUserIdAsync

        [Test]
        public async Task GetUserByIdAsync_Should_Return_User_If_Exists()
        {
            //arrange
            await userRepository.RegisterUserAsync(user);

            // Act
            var retrievedUser = await userRepository.GetUserByIdAsync(user.Id);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.That(retrievedUser.Id, Is.EqualTo(user.Id));
        }

        [Test]
        public void GetUserByIdAsync_Should_Throw_If_User_Not_Exists()
        {
            //Act Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await userRepository.GetUserByIdAsync(user.Id));
        }

        //GetUserByUsernameAsync

        [Test]
        public async Task GetUserByUsernameAsync_Should_Return_User_If_Exists()
        {
            //arrange
            await userRepository.RegisterUserAsync(user);

            // Act
            var retrievedUser = await userRepository.GetUserByUsernameAsync(user.Username);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.That(retrievedUser.Id, Is.EqualTo(user.Id));
        }

        [Test]
        public void GetUserByUsernameAsync_Should_Throw_If_User_Not_Exists()
        {
            //Act Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await userRepository.GetUserByUsernameAsync(user.Username));
        }

        //UpdateUserAsync

        [Test]
        public void UpdateUserAsync_Should_Not_Update_If_User_Was_Not_Found()
        {
            //Assert
            Assert.IsFalse(dbContext.Users.Contains(user));
            //act assert
            Assert.ThrowsAsync<MissingPrimaryKeyException>(async () => await userRepository.UpdateUserAsync(user));
        }

        [Test]
        public async Task UpdateUserAsync_Should_Not_Update_If_User_Username_Exists()
        {
            //Arrange
            User user2 = new User()
            {
                Username = "Username",
                Email = "NotTheSameEmail@mail.com",
                Password = "Password",
                RegisterDate = DateTime.Now,
            };
            await userRepository.RegisterUserAsync(user);

            //Assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.ThrowsAsync<ConstraintException>(async () => await userRepository.UpdateUserAsync(user2));
        }

        [Test]
        public async Task UpdateUserAsync_Should_Not_Update_If_User_Email_Exists()
        {
            //Arrange
            User user2 = new User()
            {
                Username = "UsernameNotTheSame",
                Email = "Email@mail.com",
                Password = "Password",
                RegisterDate = DateTime.Now,
            };
            await userRepository.RegisterUserAsync(user);

            //Assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            //act assert
            Assert.ThrowsAsync<ConstraintException>(async () => await userRepository.UpdateUserAsync(user2));
        }

        [Test]
        public async Task UpdateUserAsync_Should_Update_If_Successfully() {

            //Arrange
            await userRepository.RegisterUserAsync(user);
            user.Username = "UpdatedUsername";
            user.Email = "UpdatedEmail";
            //act
            await userRepository.UpdateUserAsync(user);

            //Assert
            Assert.IsTrue(dbContext.Users.Contains(user));
            User updatedUser = await userRepository.GetUserByUsernameAsync(user.Username);
            Assert.That(user, Is.EqualTo(updatedUser));
            Assert.That(updatedUser.Username, Is.EqualTo("UpdatedUsername"));
            Assert.That(updatedUser.Email, Is.EqualTo("UpdatedEmail"));
        }

        [Test]
        public async Task GetCurrentPageAsync_Test()
        {
            //Arrange
            User user2 = new User()
            {
                Username = "Username2",
                Email = "Email2@mail.com",
                Password = "Password",
                RegisterDate = DateTime.Now.AddMonths(2),
            };

            User user3 = new User()
            {
                Username = "Username3",
                Email = "Email3@mail.com",
                Password = "Password",
                RegisterDate = DateTime.Now.AddMonths(3),
            };

            User user4 = new User()
            {
                Username = "Username4",
                Email = "Email4@mail.com",
                Password = "Password",
                RegisterDate = DateTime.Now.AddMonths(4),
            };

            QueryParameterClass parameters = new QueryParameterClass()
            {
                SearchString = user2.Username,
                CurrentPage = 1,
                ItemsPerPage = 3,
                OrderPageEnum = Common.OrderPageEnum.Newest,
            };

            await userRepository.RegisterUserAsync(user);
            await userRepository.RegisterUserAsync(user2);
            await userRepository.RegisterUserAsync(user3);
            await userRepository.RegisterUserAsync(user4);


            //Assert 
            Assert.IsTrue(dbContext.Users.Contains(user));
            Assert.IsTrue(dbContext.Users.Contains(user2));
            Assert.IsTrue(dbContext.Users.Contains(user3));
            Assert.IsTrue(dbContext.Users.Contains(user4));

            //Assert - Search string
            QueryPageWrapClass<User> resultSearchString = await userRepository.GetUserCurrentPageAsync(parameters);
            Assert.That(resultSearchString.TotalItems, Is.EqualTo(1));
            Assert.That(resultSearchString.Items.Count, Is.EqualTo(1));
            Assert.That(resultSearchString.Items.First(), Is.EqualTo(user2));


            //Act
            parameters.SearchString = null;

            //Assert - General - Newest
            QueryPageWrapClass<User> resultGeneral = await userRepository.GetUserCurrentPageAsync(parameters);
            Assert.That(resultGeneral.TotalItems, Is.EqualTo(4));
            Assert.That(resultGeneral.Items.Count, Is.EqualTo(3));
            Assert.IsTrue(resultGeneral.Items.Contains(user2));
            Assert.IsTrue(resultGeneral.Items.Contains(user3));
            Assert.IsTrue(resultGeneral.Items.Contains(user));
        }


    }
}
