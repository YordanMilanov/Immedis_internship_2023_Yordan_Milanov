using AutoFixture;
using HCMS.Data.Models;

namespace HCMS.Tests.AutoFixtureCustomization.User
{
    internal class UserCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            // Customize the creation of User objects
            fixture.Customize<Data.Models.User>(composer =>
                composer
                    .With(u => u.Id, Guid.NewGuid())
                    .With(u => u.Username, fixture.Create<string>())
                    .With(u => u.Email, fixture.Create<string>())
                    .With(u => u.Password, fixture.Create<string>())
                    .With(u => u.RegisterDate, DateTime.Now)
                    .Without(u => u.UsersRoles)
                    .Without(u => u.UserClaims));
        }
    }
}
