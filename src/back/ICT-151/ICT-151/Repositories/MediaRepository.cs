using Azure.Storage.Blobs;
using ICT_151.Data;
using ICT_151.Exceptions;
using ICT_151.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Repositories
{
    public interface IMediaRepository
    {
        Task GetMedia(Guid id);

        Task UploadMedia(CreateMediaDTO dto);
    }

    public class MediaRepository : IMediaRepository
    {
        private ApplicationDbContext DbContext;
        private BlobServiceClient BlobServiceClient;

        public MediaRepository(ApplicationDbContext dbContext, BlobServiceClient blobServiceClient)
        {
            DbContext = dbContext;
            BlobServiceClient = blobServiceClient;
        }

        public async Task GetMedia(Guid id)
        {
            var media = DbContext.Medias.SingleOrDefault(x => x.Id == id);

            if (media == null)
                throw new DataNotFoundException($"Could not find media with id {id}");

            if (media.MediaType == MediaType.Image) {
                
            }
        }

        public Task UploadMedia(CreateMediaDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
