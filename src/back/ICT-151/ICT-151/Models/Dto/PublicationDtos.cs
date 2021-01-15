using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models.Dto
{
    public class PublicationViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public SubmissionType SubmissionType { get; set; }

        public string TextContent { get; set; }

        public string MediaUrl { get; set; }

        public int RepliesAmount { get; set; }

        public int RetweetsAmount { get; set; }

        public int LikesAmount { get; set; }

        public bool Liked { get; set; }

        public bool Reposted { get; set; }

        public UserSummaryViewModel User { get; set; }

        public Guid? ReplyPublicationId { get; set; }


        public static PublicationViewModel FromPublication(Publication publication, Guid? userId = null)
        {
            return new PublicationViewModel
            {
                Id = publication.Id,

                CreationDate = publication.CreationDate,
                SubmissionType = publication.SubmissionType,
                TextContent = publication.TextContent,
                MediaUrl = publication.MediaUrl,

                ReplyPublicationId = publication.ReplyPublicationId,
                User = UserSummaryViewModel.FromUser(publication.User),

                RepliesAmount = publication.Replies != null ? publication.Replies.Count : -1,
                RetweetsAmount = publication.Reposts != null ? publication.Reposts.Count : -1,
                LikesAmount = publication.Likes != null ? publication.Likes.Count : -1,

                Reposted = userId.HasValue && publication.Reposts.Any(x => x.UserId == userId),
                Liked = userId.HasValue && publication.Likes.Any(x => x.UserId == userId)
            };
        }
    }

    public class PublicationCreateDto
    {
        [Required]
        public SubmissionType SubmissionType { get; set; }

        [Required, StringLength(280, MinimumLength = 1)]
        public string TextContent { get; set; }

        public string MediaUrl { get; set; }

        public Guid? ReplyPublicationId { get; set; }
    }
}
