using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    public class Follow
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The user following another user
        /// </summary>
        public User Follower { get; set; }

        /// <summary>
        /// The user following another user
        /// </summary>
        public Guid FollowerId { get; set; }

        /// <summary>
        /// The user being followed
        /// </summary>
        public User FollowTarget { get; set; }

        /// <summary>
        /// The user being followed
        /// </summary>
        public Guid FollowTargetId { get; set; }
    }
}
