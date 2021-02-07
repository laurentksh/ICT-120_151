using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models.Dto
{
    public class CreateMediaDTO
    {
        [Required]
        public IFormFile Media { get; set; }

        [Required]
        public MediaContainer Container { get; set; }
    }

    public class MediaViewModel
    {
        public Guid Id { get; set; }

        public MediaType MediaType { get; set; }

        public string MimeType { get; set; }

        public long FileSize { get; set; }

        public string BlobFullUrl { get; set; }

        public UserSummaryViewModel Owner { get; set; }

        public static MediaViewModel FromMedia(Media media) => new MediaViewModel
        {
            Id = media.Id,
            MediaType = media.MediaType,
            MimeType = media.MimeType,
            FileSize = media.FileSize,
            Owner = UserSummaryViewModel.FromUser(media.Owner)
        };
    }
}
