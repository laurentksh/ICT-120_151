using ICT_151.Data;
using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;
using ICT_151.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ICT_151.Repositories
{
    public interface IMediaRepository
    {
        Task<MediaViewModel> GetMedia(Guid? userId, Guid mediaId);

        Task<bool> HasAccess(Guid? userId, Guid mediaId);

        Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO createMediaDTO);

        Task DeleteMedia(Guid mediaId);

        Task DeleteUnusedMedias(Guid userId);

        Task<bool> Exists(Guid mediaId);
    }

    public class MediaRepository : IMediaRepository
    {
        private const int MEDIA_PROCESS_TIMEOUT = 30;

        private readonly ApplicationDbContext DbContext;
        private readonly IAzureBlobStorageRepository BlobStorageRepo;
        private readonly ILogger<MediaRepository> Logger;

        public MediaRepository(ApplicationDbContext dbContext, IAzureBlobStorageRepository blobStorageRepo, ILogger<MediaRepository> logger)
        {
            DbContext = dbContext;
            BlobStorageRepo = blobStorageRepo;
            Logger = logger;
        }

        public async Task<MediaViewModel> GetMedia(Guid? userId, Guid id)
        {
            var media = DbContext.Medias
                .Include(x => x.Owner)
                .SingleOrDefault(x => x.Id == id);

            if (media == null)
                throw new DataNotFoundException($"Could not find media with id {id}");
            
            var mediaViewModel = MediaViewModel.FromMedia(media);
            mediaViewModel.BlobFullUrl = (await BlobStorageRepo.GetMediaUri(media.BlobName, media.Container)).AbsoluteUri;

            return mediaViewModel;
        }

        public async Task<bool> HasAccess(Guid? userId, Guid mediaId)
        {
            var media = DbContext.Medias
                .Include(x => x.PrivateMessage)
                .Single(x => x.Id == mediaId);

            if (media.Container != MediaContainer.PrivateMessage)
                return true;

            if (userId == null)
                return false;

            if (media.PrivateMessage.RecipientId == userId || media.PrivateMessage.SenderId == userId)
                return true;
            else
                return false;
        }

        public async Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO createMediaDTO)
        {
            await DeleteUnusedMedias(userId);

            using var ms = new MemoryStream();
            await createMediaDTO.Media.CopyToAsync(ms, new CancellationTokenSource(TimeSpan.FromSeconds(MEDIA_PROCESS_TIMEOUT)).Token);

            var blobName = $"media-{Guid.NewGuid()}";
            
            ms.Position = 0;
            var result = await BlobStorageRepo.UploadMedia(ms, blobName, createMediaDTO.Container);

            var media = new Media
            {
                MediaType = createMediaDTO.Media.ContentType.ToMediaType(),
                MimeType = createMediaDTO.Media.ContentType,
                BlobName = blobName,
                Container = createMediaDTO.Container,
                FileSize = createMediaDTO.Media.Length,
                OwnerId = userId
            };

            await DbContext.Medias.AddAsync(media);
            await DbContext.SaveChangesAsync();

            var mediaViewModel = MediaViewModel.FromMedia(media);
            mediaViewModel.BlobFullUrl = result.AbsoluteUri;

            return mediaViewModel;
        }

        public async Task DeleteMedia(Guid mediaId)
        {
            DbContext.Medias.Remove(await DbContext.Medias.SingleAsync(x => x.Id == mediaId));
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteUnusedMedias(Guid userId)
        {
            var medias = DbContext.Medias
                .Include(x => x.User)
                .Include(x => x.Publication)
                .Include(x => x.PrivateMessage)
                .Where(x => x.OwnerId == userId)
                .ToList()
                .Where(x => x.User == null && x.Publication == null && x.PrivateMessage == null);

            DbContext.Medias.RemoveRange(medias);
        }

        public async Task<bool> Exists(Guid mediaId)
        {
            return await DbContext.Medias.AnyAsync(x => x.Id == mediaId);
        }
    }
}
