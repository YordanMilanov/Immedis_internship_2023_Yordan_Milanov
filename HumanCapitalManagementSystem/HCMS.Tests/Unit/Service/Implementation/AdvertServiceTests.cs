using AutoMapper;
using HCMS.Repository.Interfaces;
using HCMS.Services.Implementation;
using HCMS.Services.Interfaces;
using Moq;

namespace HCMS.UnitTest.Service.Implementation
{
    [TestFixture]
    internal class AdvertServiceTests
    {
        private IAdvertService advertService;
        private Mock<IAdvertRepository> mockAdvertRepository;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            mapper = AutoMapperHelper.InitializeMapper();
            mockAdvertRepository = new Mock<IAdvertRepository>();
        }
    }
}
