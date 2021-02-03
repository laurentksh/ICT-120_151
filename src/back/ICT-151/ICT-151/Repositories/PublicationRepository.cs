using ICT_151.Data;
using ICT_151.Models;
using ICT_151.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ICT_151.Repositories
{
    public interface IPublicationRepository
    {
        Task<PublicationViewModel> GetPublication(Guid id, Guid? requestUserId);

        Task<IEnumerable<PublicationViewModel>> GetReplies(Guid id, Guid? requestUserId);

        Task<IEnumerable<RepostViewModel>> GetReposts(Guid publicationId);

        Task<IEnumerable<LikeViewModel>> GetLikes(Guid publicationId);

        Task<PublicationViewModel> CreateNew(Guid userId, PublicationCreateDto publication);

        Task Repost(Guid userId, Guid publicationId);

        Task Like(Guid userId, Guid publicationId);

        Task UnRepost(Guid userId, Guid publicationId);

        Task UnLike(Guid userId, Guid publicationId);

        Task<bool> Exists(Guid id);

        Task<bool> RepostExists(Guid userId, Guid publicationId);

        Task<bool> LikeExists(Guid userId, Guid publicationId);
    }

    public class PublicationRepository : IPublicationRepository
    {
        private ApplicationDbContext DbContext;

        public PublicationRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<PublicationViewModel> GetPublication(Guid id, Guid? requestUserId)
        {
            var publication = DbContext.Publications
                .Include(x => x.User)
                .Include(x => x.Replies)
                .Include(x => x.Reposts)
                .Include(x => x.Likes)
                .Single(x => x.Id == id);

            return PublicationViewModel.FromPublication(publication, requestUserId);

        }

        public async Task<IEnumerable<PublicationViewModel>> GetReplies(Guid id, Guid? requestUserId)
        {
            return DbContext.Publications
                .OrderBy(x => x.CreationDate)
                .Include(x => x.User)
                .Include(x => x.Replies)
                .Include(x => x.Reposts)
                .Include(x => x.Likes)
                .Where(x => x.ReplyPublicationId == id)
                .Select(x => PublicationViewModel.FromPublication(x, requestUserId));
        }
        
        public async Task<IEnumerable<RepostViewModel>> GetReposts(Guid publicationId)
        {
            return DbContext.Reposts
                .OrderBy(x => x.CreationDate)
                .Include(x => x.User)
                .Where(x => x.PublicationId == publicationId)
                .Select(x => RepostViewModel.FromRepost(x));
        }

        public async Task<IEnumerable<LikeViewModel>> GetLikes(Guid publicationId)
        {
            return DbContext.Likes
                .Include(x => x.User)
                .Where(x => x.PublicationId == publicationId)
                .Select(x => LikeViewModel.FromLike(x));
        }

        public async Task<PublicationViewModel> CreateNew(Guid userId, PublicationCreateDto publication)
        {
            var result = await DbContext.Publications.AddAsync(new Publication
            {
                SubmissionType = publication.SubmissionType,
                TextContent = publication.TextContent,
                MediaId = publication.MediaId,
                ReplyPublicationId = publication.ReplyPublicationId,

                CreationDate = DateTime.UtcNow,
                UserId = userId
            });
            await DbContext.SaveChangesAsync();

            return PublicationViewModel.FromPublication(result.Entity);
        }

        public async Task Repost(Guid userId, Guid publicationId)
        {
            await DbContext.Reposts.AddAsync(new Repost
            {
                UserId = userId,
                PublicationId = publicationId,
                CreationDate = DateTime.UtcNow
            });
            await DbContext.SaveChangesAsync();
        }

        public async Task Like(Guid userId, Guid publicationId)
        {
            await DbContext.Likes.AddAsync(new Like
            {
                UserId = userId,
                PublicationId = publicationId,
                CreationDate = DateTime.UtcNow
            });
            await DbContext.SaveChangesAsync();
        }

        public async Task UnRepost(Guid userId, Guid publicationId)
        {
            DbContext.Reposts.Remove(DbContext.Reposts.Single(x => x.PublicationId == publicationId && x.UserId == userId));
            await DbContext.SaveChangesAsync();
        }

        public async Task UnLike(Guid userId, Guid publicationId)
        {
            DbContext.Likes.Remove(DbContext.Likes.Single(x => x.PublicationId == publicationId && x.UserId == userId));
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await DbContext.Publications.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> RepostExists(Guid userId, Guid publicationId)
        {
            return await DbContext.Reposts.AnyAsync(x => x.UserId == userId && x.PublicationId == publicationId);
        }

        public async Task<bool> LikeExists(Guid userId, Guid publicationId)
        {
            return await DbContext.Likes.AnyAsync(x => x.UserId == userId && x.PublicationId == publicationId);
        }
    }
}
