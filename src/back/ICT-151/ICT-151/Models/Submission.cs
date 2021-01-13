using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    public class Submission
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public SubmissionType SubmissionType { get; set; }

        public string TextContent { get; set; }

        public string MediaUrl { get; set; } //I could have made a media table in my DB but it would take too much time so i'm only sending the url (%domain%/api/Media/image/%guid%) which returns the media from the file manager.

        public Guid UserId { get; set; }

        public User User { get; set; }
    }

    public enum SubmissionType
    {
        Text,
        Image,
        Video,
        Poll
    }
}
