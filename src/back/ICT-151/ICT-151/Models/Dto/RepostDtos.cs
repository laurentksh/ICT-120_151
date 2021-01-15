using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models.Dto
{
    public class RepostViewModel
    {
        public Guid Id { get; set; }

        public UserSummaryViewModel User { get; set; }


        public static RepostViewModel FromRepost(Repost repost)
        {
            return new RepostViewModel
            {
                Id = repost.Id,
                User = UserSummaryViewModel.FromUser(repost.User)
            };
        }
    }
}