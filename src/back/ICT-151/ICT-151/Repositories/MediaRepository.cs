using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ICT_151.Data;
using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;
using ICT_151.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ICT_151.Repositories
{
    public interface IMediaRepository
    {
        /// <summary>
        /// Returns the media with it's URL.
        /// </summary>
        /// <param name="userId">User accessing the media.</param>
        /// <param name="mediaId">Media Id.</param>
        /// <param name="container">Media container to use.</param>
        /// <returns>Media view model</returns>
        Task<MediaViewModel> GetMedia(Guid? userId, Guid mediaId);

        /// <summary>
        /// Check whether an user has access to a media.
        /// </summary>
        /// <param name="userId">User accessing the media.</param>
        /// <param name="mediaId">Media Id.</param>
        /// <returns>true: authorized; false: forbidden</returns>
        Task<bool> HasAccess(Guid? userId, Guid mediaId);

        /// <summary>
        /// Checks whether a media exists.
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns></returns>
        Task<bool> Exists(Guid mediaId);

        /// <summary>
        /// Upload a media to Azure Blob Storage and create a database entry.
        /// </summary>
        /// <param name="userId">User uploading the file.</param>
        /// <param name="dto">Create media DTO.</param>
        /// <param name="container">Media container to use.</param>
        /// <returns>Media view model</returns>
        Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO dto, MediaContainer container);
    }

    public class MediaRepository : IMediaRepository
    {
        private const int MEDIA_PROCESS_TIMEOUT = 30;

        private ApplicationDbContext DbContext;
        private IAzureBlobStorageRepository BlobStorageRepo;

        public MediaRepository(ApplicationDbContext dbContext, IAzureBlobStorageRepository blobStorageRepo)
        {
            DbContext = dbContext;
            BlobStorageRepo = blobStorageRepo;
        }

        public async Task<MediaViewModel> GetMedia(Guid? userId, Guid id)
        {
            var media = DbContext.Medias
                .Include(x => x.Owner)
                .SingleOrDefault(x => x.Id == id);

            if (media == null)
                throw new DataNotFoundException($"Could not find media with id {id}");
            
            var vm = MediaViewModel.FromMedia(media);
            vm.BlobFullUrl = (await BlobStorageRepo.GetMediaUri(media.BlobName, media.Container)).AbsoluteUri;

            return vm;
        }

        public async Task<bool> HasAccess(Guid? userId, Guid mediaId)
        {
            var media = DbContext.Medias
                //.Include(x => x.PrivateMessage)
                .Single(x => x.Id == mediaId);

            if (userId == null)
                return false;

            return media.OwnerId == userId;

            /*if (media.Container != MediaContainer.PrivateMessage)
                return true;

            if (userId == null || media.PrivateMessage.RecipientId == userId || media.PrivateMessage.SenderId == userId)
                return true;
            else
                return false;*/
        }

        public async Task<bool> Exists(Guid mediaId)
        {
            return await DbContext.Medias.AnyAsync(x => x.Id == mediaId);
        }

        public async Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO dto, MediaContainer containerType)
        {
            using var ms = new MemoryStream();
            await dto.Media.CopyToAsync(ms, new CancellationTokenSource(TimeSpan.FromSeconds(MEDIA_PROCESS_TIMEOUT)).Token);

            var blobName = $"media-{Guid.NewGuid()}";

            var result = await BlobStorageRepo.UploadMedia(ms, blobName, containerType);

            var media = new Media
            {
                MediaType = dto.Media.ContentType.ToMediaType(),
                MimeType = dto.Media.ContentType,
                BlobName = blobName,
                Container = containerType,
                FileSize = dto.Media.Length,
                OwnerId = userId
            };

            await DbContext.Medias.AddAsync(media);
            await DbContext.SaveChangesAsync();

            return MediaViewModel.FromMedia(media);
        }
    }
}
