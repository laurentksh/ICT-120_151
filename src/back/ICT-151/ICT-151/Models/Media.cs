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

        [InverseProperty(nameof(Models.User.ProfilePictureMedia))]
        public User User { get; set; }

        [InverseProperty(nameof(Models.Publication.Media))]
        public Publication Publication { get; set; }

        [InverseProperty(nameof(Models.PrivateMessage.Media))]
        public PrivateMessage PrivateMessage { get; set; }


        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }

        public User Owner { get; set; }
    }

    public enum MediaType
    {
        Unknown = 0,
        Image = 1,
        Video = 2
    }

    public enum MediaContainer
    {
        Unknown = 0,
        Publication = 1,
        ProfilePicture = 2,
        PrivateMessage = 3
    }
}
