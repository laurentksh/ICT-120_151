using ICT_151.Exceptions;
using ICT_151.Models.Dto;
using ICT_151.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Services
{
    public interface IFeedService
    {
        int DefaultPublicationAmount { get; }

        int MaxPublicationAmount { get; }

        Task<List<PublicationViewModel>> GetMainFeed(int amount, Guid? positionId, Guid? requestUserId);

        Task<List<PublicationViewModel>> GetFeed(Guid userId, int amount, Guid? positionId, Guid? requestUserId);

        Task<List<PublicationViewModel>> GetFeed(string username, int amount, Guid? positionId, Guid? requestUserId);
    }

    public class FeedService : IFeedService
    {
        public int DefaultPublicationAmount { get; } = 100;
        public int MaxPublicationAmount { get; } = 200;

        private IUserService UserService;
        private IPublicationService PublicationService;
        private IFeedRepository FeedRepository;

        public FeedService(IUserService userService, IPublicationService publicationService, IFeedRepository feedRepository)
        {
            UserService = userService;
            PublicationService = publicationService;
            FeedRepository = feedRepository;
        }

        public async Task<List<PublicationViewModel>> GetMainFeed(int amount, Guid? positionId, Guid? requestUserId)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0");
            if (amount > MaxPublicationAmount)
                throw new ArgumentOutOfRangeException(nameof(amount), $"Amount cannot be greater than {MaxPublicationAmount}");

            if (positionId.HasValue && !await PublicationService.Exists(positionId.Value))
                throw new DataNotFoundException($"{positionId.Value} does not exists.");

            return (await FeedRepository.GetMainFeed(amount, positionId, requestUserId)).ToList();
        }

        public async Task<List<PublicationViewModel>> GetFeed(Guid userId, int amount, Guid? positionId, Guid? requestUserId)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0");
            if (amount > MaxPublicationAmount)
                throw new ArgumentOutOfRangeException(nameof(amount), $"Amount cannot be greater than {MaxPublicationAmount}");

            if (!await UserService.Exists(userId))
                throw new UserNotFoundException("User not found");
            if (positionId.HasValue && !await PublicationService.Exists(positionId.Value))
                throw new DataNotFoundException($"{positionId.Value.ToString()} does not exists.");

            return (await FeedRepository.GetFeed(userId, amount, positionId, requestUserId)).ToList();
        }

        public async Task<List<PublicationViewModel>> GetFeed(string username, int amount, Guid? positionId, Guid? requestUserId)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than 0");
            if (amount > MaxPublicationAmount)
                throw new ArgumentOutOfRangeException(nameof(amount), $"Amount cannot be greater than {MaxPublicationAmount}");
            
            if (!await UserService.Exists(username))
                throw new UserNotFoundException("Username not found");
            if (positionId.HasValue && !await PublicationService.Exists(positionId.Value))
                throw new DataNotFoundException($"{positionId.Value} does not exists.");

            return (await FeedRepository.GetFeed((await UserService.GetUser(username)).Id, amount, positionId, requestUserId)).ToList();
        }
    }
}
