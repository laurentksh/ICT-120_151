using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models.Dto
{
    public class LikeViewModel
    {
        public Guid Id { get; set; }

        public UserSummaryViewModel User { get; set; }


        public static LikeViewModel FromLike(Like like)
        {
            return new LikeViewModel
            {
                Id = like.Id,
                User = UserSummaryViewModel.FromUser(like.User)
            };
        }
    }
}
