using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;
using ICT_151.Repositories;
using ICT_151.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Services
{
    public interface IMediaService
    {
        Task<MediaViewModel> GetMedia(Guid? userId, Guid id);

        Task<MediaViewModel> GetDefaultProfilePicture();

        Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO dto, MediaContainer container);
    }

    public class MediaService : IMediaService
    {
        private IMediaRepository Repository;

        public MediaService(IMediaRepository repo)
        {
            Repository = repo;
        }

        public async Task<MediaViewModel> GetMedia(Guid? userId, Guid id)
        {
            if (await Repository.HasAccess(userId, id))
                throw new ForbiddenException($"You do not have access to media '${id}'.");

            if (await Repository.Exists(id))
                throw new DataNotFoundException($"Media with id '${id}' does not exist.");

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

        public async Task<MediaViewModel> UploadMedia(Guid userId, CreateMediaDTO dto, MediaContainer container)
        {
            if (dto.Media.ContentType.ToMediaType() == MediaType.Unknown)
                throw new ArgumentException($"Invalid file type '{dto.Media.ContentType}'.", nameof(dto));
            if (container == MediaContainer.Unknown)
                throw new ArgumentException("Media container not found/must have a value.", nameof(container));

            return await Repository.UploadMedia(userId, dto, container);
        }
    }
}
