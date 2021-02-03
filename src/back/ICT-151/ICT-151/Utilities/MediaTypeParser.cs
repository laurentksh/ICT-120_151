using ICT_151.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Utilities
{
    public static class MediaTypeUtilities
    {
        public static MediaType ToMediaType(this string contentType)
        {
            return contentType switch
            {
                //Images
                "image/jpeg" => MediaType.Image,
                "image/png" => MediaType.Image,
                "image/webp" => MediaType.Image,
                //Animated images
                "image/gif" => MediaType.Image,

                //Videos
                "video/mp4" => MediaType.Video,
                "video/webm" => MediaType.Video,
                "video/ogg" => MediaType.Video,
                _ => MediaType.Unknown
            };
        }
    }
}
