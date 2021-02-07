using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;
using ICT_151.Repositories;
using ICT_151.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Services
{
    public interface IMediaService
    {
        /// <summary>
        /// Returns the media as a MediaViewModel.
        /// </summary>
        /// <param name="userId">User accessing the media.</param>
        /// <param name="id">Media Id.</param>
        /// <returns>A MediaViewModel for the specified media Id.</returns>
        Task<MediaViewModel> GetMedia(Guid? userId, Guid id);

        /// <summary>
        /// Returns a MediaViewModel object for the default profile picture (this method does not query the database).
        /// </summary>
        /// <returns>A MediaViewModel</returns>
        Task<MediaViewModel> GetDefaultProfilePicture();

        /// <summary>
        /// Check whether an user has access to a media.
        /// </summary>
        /// <param name="userId">User accessing the media.</param>
        /// <param name="mediaId">Media Id.</param>
        /// <returns>true: authorized; false: forbidden</returns>
        Task<bool> HasAccess(Guid? userId, Guid mediaId);

        /// <summary>
        /// Upload a media to Azure Blob Storage and create a database entry.
        /// </summary>
        /// <param name="userId">User uploading the file.</param>
        /// <param name="file">File to store.</param>
        /// <param name="container">Media container to use.</param>
        /// <returns>The MediaViewModel of the newly created file.</returns>
        Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO createMediaDTO);

        /// <summary>
        /// Delete a media, both from the database and Azure Blob Storage.
        /// </summary>
        /// <param name="mediaId">Media to delete</param>
        /// <returns></returns>
        Task DeleteMedia(Guid mediaId);

        /// <summary>
        /// Checks whether a media exists.
        /// </summary>
        /// <param name="mediaId"></param>
        /// <returns>true: the media exist; false: it does not.</returns>
        Task<bool> Exists(Guid mediaId);
    }

    public class MediaService : IMediaService
    {
        private readonly IMediaRepository Repository;

        public MediaService(IMediaRepository repo)
        {
            Repository = repo;
        }

        public async Task<MediaViewModel> GetMedia(Guid? userId, Guid id)
        {
            if (!await Repository.HasAccess(userId, id))
                throw new ForbiddenException($"You do not have access to media '{id}'.");

            if (!await Repository.Exists(id))
                throw new DataNotFoundException($"Media with id '{id}' does not exist.");

            var media = await Repository.GetMedia(userId, id);
            return media;
        }

        public async Task<MediaViewModel> GetDefaultProfilePicture()
        {
            return new MediaViewModel
            {
                Id = Guid.Empty,
                MediaType = MediaType.Image,
                BlobFullUrl = "/assets/default_pp.svg",
                FileSize = 225,
                MimeType = "image/svg",
                Owner = null
            };
        }

        public async Task<bool> HasAccess(Guid? userId, Guid mediaId)
        {
            return await Repository.HasAccess(userId, mediaId);
        }

        public async Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO createMediaDTO)
        {
            if (createMediaDTO.Media.ContentType.ToMediaType() == MediaType.Unknown)
                throw new UnsupportedMediaTypeException($"Invalid file type '{createMediaDTO.Media.ContentType}'.");
            if (createMediaDTO.Container == MediaContainer.Unknown)
                throw new ArgumentException("Media container not found/must have a value.", nameof(createMediaDTO));

            return await Repository.UploadMedia(userId, createMediaDTO);
        }

        public async Task DeleteMedia(Guid mediaId)
        {
            if (!await Exists(mediaId))
                throw new DataNotFoundException($"Media with id '{mediaId}' not found.");

            await Repository.DeleteMedia(mediaId);
        }

        public async Task<bool> Exists(Guid mediaId)
        {
            return await Repository.Exists(mediaId);
        }
    }
}
