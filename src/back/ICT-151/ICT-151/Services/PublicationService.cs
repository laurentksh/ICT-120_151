using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;
using ICT_151.Repositories;

namespace ICT_151.Services
{
    public interface IPublicationService
    {
        Task<PublicationViewModel> GetPublication(Guid id, Guid? requestUserId);

        Task<List<PublicationViewModel>> GetReplies(Guid id, Guid? requestUserId);

        Task<List<RepostViewModel>> GetReposts(Guid publicationId);

        Task<List<LikeViewModel>> GetLikes(Guid publicationId);

        Task<PublicationViewModel> CreateNew(Guid userId, PublicationCreateDto publication);

        Task Remove(Guid userId, Guid publicationId);

        Task Repost(Guid userId, Guid publicationId);

        Task Like(Guid userId, Guid publicationId);

        Task UnRepost(Guid userId, Guid publicationId);

        Task UnLike(Guid userId, Guid publicationId);

        Task<bool> Exists(Guid id);
    }

    public class PublicationService : IPublicationService
    {
        private readonly IPublicationRepository PublicationRepository;
        private readonly IUserService UserService;

        public PublicationService(IPublicationRepository publicationRepository, IUserService userService)
        {
            PublicationRepository = publicationRepository;
            UserService = userService;
        }

        public async Task<PublicationViewModel> GetPublication(Guid id, Guid? requestUserId)
        {
            if (!await PublicationRepository.Exists(id))
                throw new DataNotFoundException("Publication does not exist.");

            return await PublicationRepository.GetPublication(id, requestUserId);
        }

        public async Task<List<PublicationViewModel>> GetReplies(Guid id, Guid? requestUserId)
        {
            if (!await PublicationRepository.Exists(id))
                throw new DataNotFoundException("Publication does not exist.");

            return (await PublicationRepository.GetReplies(id, requestUserId)).ToList();
        }

        public async Task<List<RepostViewModel>> GetReposts(Guid publicationId)
        {
            if (!await PublicationRepository.Exists(publicationId))
                throw new DataNotFoundException("Publication does not exist.");

            return (await PublicationRepository.GetReposts(publicationId)).ToList();
        }

        public async Task<List<LikeViewModel>> GetLikes(Guid publicationId)
        {
            if (!await PublicationRepository.Exists(publicationId))
                throw new DataNotFoundException("Publication does not exist.");

            return (await PublicationRepository.GetLikes(publicationId)).ToList();
        }

        public async Task<PublicationViewModel> CreateNew(Guid userId, PublicationCreateDto publication)
        {
            if (publication.ReplyPublicationId.HasValue && !await PublicationRepository.Exists(publication.ReplyPublicationId.Value))
                throw new DataNotFoundException($"Target publication {publication.ReplyPublicationId} does not exist.");

            return await PublicationRepository.CreateNew(userId, publication);
        }

        public async Task Remove(Guid userId, Guid publicationId)
        {
            if (!await Exists(publicationId))
                throw new DataNotFoundException("Publication does not exist.");

            await PublicationRepository.Remove(userId, publicationId);
        }

        public async Task Repost(Guid userId, Guid publicationId)
        {
            if (!await Exists(publicationId))
                throw new DataNotFoundException();

            if (await PublicationRepository.RepostExists(userId, publicationId))
                throw new DataAlreadyExistsException("This publication has already been reposted.");

            await PublicationRepository.Repost(userId, publicationId);
        }

        public async Task Like(Guid userId, Guid publicationId)
        {
            if (!await Exists(publicationId))
                throw new DataNotFoundException();

            if (await PublicationRepository.LikeExists(userId, publicationId))
                throw new DataAlreadyExistsException("This publication has already been liked.");

            await PublicationRepository.Like(userId, publicationId);
        }

        public async Task UnRepost(Guid userId, Guid publicationId)
        {
            if (!await Exists(publicationId) ||
                !await PublicationRepository.RepostExists(userId, publicationId))
                throw new DataNotFoundException();

            await PublicationRepository.UnRepost(userId, publicationId);
        }

        public async Task UnLike(Guid userId, Guid publicationId)
        {
            if (!await Exists(publicationId) ||
                !await PublicationRepository.LikeExists(userId, publicationId))
                throw new DataNotFoundException();

            await PublicationRepository.UnLike(userId, publicationId);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await PublicationRepository.Exists(id);
        }
    }
}
