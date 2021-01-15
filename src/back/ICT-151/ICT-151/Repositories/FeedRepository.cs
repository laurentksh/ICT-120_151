using ICT_151.Data;
using ICT_151.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Repositories
{
    public interface IFeedRepository
    {
        Task<IEnumerable<PublicationViewModel>> GetMainFeed(int amount, Guid? positionId, Guid? requestUserId);

        Task<IEnumerable<PublicationViewModel>> GetFeed(Guid userId, int amount, Guid? positionId, Guid? requestUserId);
    }

    public class FeedRepository : IFeedRepository
    {
        private ApplicationDbContext DbContext;

        public FeedRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<IEnumerable<PublicationViewModel>> GetMainFeed(int amount, Guid? positionId, Guid? requestUserId)
        {
            var publications = DbContext.Publications
                .OrderBy(x => x.CreationDate)
                .Include(x => x.User)
                .Include(x => x.Replies)
                .Include(x => x.Reposts)
                .Include(x => x.Likes)
                .Where(x => x.ReplyPublicationId.HasValue == false);

            if (positionId.HasValue)
            {
                return publications
                    .SkipWhile(x => x.Id != positionId)
                    .Take(amount)
                    .Select(x => PublicationViewModel.FromPublication(x, requestUserId));
            } else
            {
                return publications
                    .Take(amount)
                    .Select(x => PublicationViewModel.FromPublication(x, requestUserId));
            }
        }

        public async Task<IEnumerable<PublicationViewModel>> GetFeed(Guid userId, int amount, Guid? positionId, Guid? requestUserId)
        {
            var publications = DbContext.Publications
                .Include(x => x.User)
                .Include(x => x.Replies)
                .Include(x => x.Reposts)
                .Include(x => x.Likes)
                .Where(x => x.ReplyPublicationId.HasValue == false && x.UserId == userId);

            if (positionId.HasValue)
            {
                return publications
                    .SkipWhile(x => x.Id != positionId)
                    .Take(amount)
                    .Select(x => PublicationViewModel.FromPublication(x, requestUserId));
            }
            else
            {
                return publications
                    .Take(amount)
                    .Select(x => PublicationViewModel.FromPublication(x, requestUserId));
            }
        }
    }
}
