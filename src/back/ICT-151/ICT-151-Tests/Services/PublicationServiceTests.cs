using ICT_151.Exceptions;
using ICT_151.Models.Dto;
using ICT_151.Repositories;
using ICT_151.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICT_151_Tests.Services
{
    [TestClass]
    public class PublicationServiceTests
    {
        private Mock<IPublicationRepository> repo;
        private Mock<IUserRepository> userRepo;
        private Mock<IMediaRepository> mediaRepo;
        private IPublicationService service;
        private IUserService userService;
        private IMediaService mediaService;

        private Guid ExistingPublication;
        private Guid EmptyExistingPublication;

        private Guid ExistingUser;
        private string ExistingUsername;

        [TestInitialize]
        public void Initialize()
        {
            repo = new Mock<IPublicationRepository>();
            userRepo = new Mock<IUserRepository>();
            mediaRepo = new Mock<IMediaRepository>();
            mediaService = new MediaService(mediaRepo.Object);
            userService = new UserService(userRepo.Object, mediaService);
            service = new PublicationService(repo.Object, userService);

            ExistingUser = Guid.NewGuid();
            ExistingUsername = "Abcdef1234";

            ExistingPublication = Guid.NewGuid();
            EmptyExistingPublication = Guid.NewGuid();

            repo.Setup(x => x.Exists(It.IsAny<Guid>()))
                .ReturnsAsync((Guid value) => value == ExistingPublication || value == EmptyExistingPublication);

            userRepo.Setup(x => x.Exists(It.IsAny<Guid>()))
                .ReturnsAsync((Guid value) => value == ExistingUser);

            userRepo.Setup(x => x.Exists(It.IsAny<string>()))
                .ReturnsAsync((string value) => value == ExistingUsername);
        }

        [TestMethod]
        public async Task GetPublication_When_Valid()
        {
            //ARRANGE
            repo.Setup(x => x.GetPublication(It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .ReturnsAsync((Guid value, Guid? user) =>
                {
                    if (value == ExistingPublication)
                        return new PublicationViewModel();
                    else
                        return null;
                });

            //ACT
            var result = await service.GetPublication(ExistingPublication, null);

            //ASSERT
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetPublication_When_Publication_DoesNotExist()
        {
            //ARRANGE + ACT + ASSERT
            await Assert.ThrowsExceptionAsync<DataNotFoundException>(() => service.GetPublication(Guid.NewGuid(), null));
        }

        [TestMethod]
        public async Task GetReplies_When_Valid()
        {
            //ARRANGE
            repo.Setup(x => x.GetReplies(It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .ReturnsAsync((Guid value, Guid? user) =>
                {
                    if (value == ExistingPublication)
                        return Enumerable.Repeat(new PublicationViewModel(), 5);
                    else if (value == EmptyExistingPublication)
                        return Enumerable.Empty<PublicationViewModel>();
                    else
                        return null;
                });

            //ACT
            var result = await service.GetReplies(ExistingPublication, null);

            //ASSERT
            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public async Task GetReplies_When_Publication_HasNoReplies()
        {
            //ARRANGE
            repo.Setup(x => x.GetReplies(It.IsAny<Guid>(), It.IsAny<Guid?>()))
                .ReturnsAsync((Guid value, Guid? user) =>
                {
                    if (value == ExistingPublication)
                        return Enumerable.Repeat(new PublicationViewModel(), 5);
                    else if (value == EmptyExistingPublication)
                        return Enumerable.Empty<PublicationViewModel>();
                    else
                        return null;
                });

            //ACT
            var result = await service.GetReplies(EmptyExistingPublication, null);

            //ASSERT
            Assert.AreEqual(0, result.Count);
        }
    }
}
