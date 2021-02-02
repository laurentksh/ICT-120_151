using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    public class Media
    {
        public Guid Id { get; set; }

        public MediaType MediaType { get; set; }

        public string MimeType { get; set; }

        public long FileSize { get; set; }

        public string BlobName { get; set; }


        public Guid OwnerId { get; set; }

        public User Owner { get; set; }
    }

    public enum MediaType
    {
        Unknown,
        Image,
        Video
    }

    public class CreateMediaDTO
    {

    }
}
