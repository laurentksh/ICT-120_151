using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    /// <summary>
    /// Publication, can be a reply to another publication
    /// </summary>
    public class Publication
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public SubmissionType SubmissionType { get; set; }

        [Required, StringLength(280, MinimumLength = 1)]
        public string TextContent { get; set; }

        public string MediaUrl { get; set; } //I could have made a media table in my DB but it would take too much time so i'm only sending the url (%domain%/api/Media/image/%guid%) which returns the media from the file manager.

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [ForeignKey(nameof(Publication))]
        public Guid? ReplyPublicationId { get; set; }

        public Publication ReplyPublication { get; set; }

        [InverseProperty(nameof(ReplyPublication))] //Idk if it'll works lol
        public List<Publication> Replies { get; set; }

        [InverseProperty(nameof(Repost.Publication))]
        public List<Repost> Reposts { get; set; }

        [InverseProperty(nameof(Like.Publication))]
        public List<Like> Likes { get; set; }
    }

    public enum SubmissionType
    {
        Text = 0,

        Image = 1,

        Video = 2,

        /// <summary>
        /// A reply cannot be of this type.
        /// </summary>
        Poll = 3
    }
}
