using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;
using ICT_151.Repositories;
using ICT_151.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ICT_151_Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> repo;
        private Mock<IMediaRepository> mediaRepo;
        private IMediaService mediaService;
        private IUserService service;

        private Guid ExistingUser;
        private string ExistingUsername;

        [TestInitialize]
        public void Initialize()
        {
            mediaRepo = new Mock<IMediaRepository>();
            mediaService = new MediaService(mediaRepo.Object);

            repo = new Mock<IUserRepository>();
            service = new UserService(repo.Object, mediaService);

            ExistingUser = Guid.NewGuid();
            ExistingUsername = "Abcdef1234";

            repo.Setup(x => x.Exists(It.IsAny<Guid>()))
                .ReturnsAsync((Guid value) => value == ExistingUser);

            repo.Setup(x => x.Exists(It.IsAny<string>()))
                .ReturnsAsync((string value) => value == ExistingUsername);
        }

        [TestMethod]
        public async Task Authenticate_When_Valid()
        {
            //ARRANGE
            repo.Setup(x => x.AuthenticateUser(It.IsAny<AuthUserDto>(), It.IsNotNull<IPAddress>()))
                .ReturnsAsync(new UserSession());

            var dto = new AuthUserDto()
            {
                Email = "abc@def.com",
                Password = "abc",
                ExtendSession = false
            };

            //ACT
            var result = service.AuthenticateUser(dto, IPAddress.Loopback);

            //ASSERT
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Authenticate_When_Null()
        {
            //ARRANGE
            repo.Setup(x => x.AuthenticateUser(It.IsAny<AuthUserDto>(), It.IsNotNull<IPAddress>()))
                .ReturnsAsync(new UserSession());

            AuthUserDto dto = null;

            //ACT + ASSERT
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => service.AuthenticateUser(dto, IPAddress.Loopback));
        }

        [TestMethod]
        public async Task GetSessions_When_Valid()
        {
            //ARRANGE
            repo.Setup(x => x.GetSessions(It.IsAny<Guid>()))
                .ReturnsAsync(Enumerable.Repeat(new UserSessionSummaryViewModel(), 10));

            //ACT
            var result = await service.GetSessions(ExistingUser);

            //ASSERT
            Assert.AreEqual(10, result.Count);
        }

        [TestMethod]
        public async Task GetSessions_When_User_DoesNotExist()
        {
            //ARRANGE
            repo.Setup(x => x.GetSessions(It.IsAny<Guid>()))
                .ReturnsAsync(Enumerable.Repeat(new UserSessionSummaryViewModel(), 10));

            //ACT + ASSERT
            await Assert.ThrowsExceptionAsync<UserNotFoundException>(() => service.GetSessions(Guid.NewGuid()));
        }

        [TestMethod]
        public async Task CreateNew_When_Valid()
        {
            //ARRANGE
            repo.Setup(x => x.CreateNew(It.IsAny<CreateUserDto>(), It.IsAny<IPAddress>()))
                .ReturnsAsync(new CreatedUserViewModel());

            var dto = new CreateUserDto()
            {
                Email = "abc@def.com",
                Username = "asdf",
                Password = "Abcdefghijkl12345678$$$",
                BirthDay = new DateTime(2000, 3, 1)
            };
            var ip = IPAddress.Loopback;

            //ACT
            var result = await service.CreateNew(dto, ip);

            //ASSERT
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateNew_When_Username_AlreadyExists()
        {
            //ARRANGE
            repo.Setup(x => x.CreateNew(It.IsAny<CreateUserDto>(), It.IsAny<IPAddress>()))
                .ReturnsAsync(new CreatedUserViewModel());

            var dto = new CreateUserDto()
            {
                Email = "abc@def.com",
                Username = ExistingUsername,
                Password = "Abcdefghijkl12345678$$$",
                BirthDay = new DateTime(2000, 3, 1)
            };
            var ip = IPAddress.Loopback;

            //ACT + ASSERT
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => service.CreateNew(dto, ip));
        }
    }
}
