using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    public class Media
    {
        public Guid Id { get; set; }

        public MediaType MediaType { get; set; }

        public MediaContainer Container { get; set; }

        public string MimeType { get; set; }

        public long FileSize { get; set; }

        public string BlobName { get; set; }


        [InverseProperty(nameof(Models.Publication.Media))]
        public Publication Publication { get; set; }

        [InverseProperty(nameof(Models.PrivateMessage.Media))]
        public PrivateMessage PrivateMessage { get; set; }


        public Guid OwnerId { get; set; }

        public User Owner { get; set; }
    }

    public enum MediaType
    {
        Unknown,
        Image,
        Video
    }

    public enum MediaContainer
    {
        Unknown,
        Publication,
        ProfilePicture,
        PrivateMessage
    }
}
