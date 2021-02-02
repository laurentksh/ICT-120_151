using ICT_151.Models;
using ICT_151.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Services
{
    public interface IMediaService
    {
        Task GetMedia(Guid id);

        Task UploadMedia(CreateMediaDTO dto);
    }

    public class MediaService : IMediaService
    {
        private IMediaRepository Repository;

        public MediaService(IMediaRepository repo)
        {
            Repository = repo;
        }

        public Task GetMedia(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UploadMedia(CreateMediaDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
