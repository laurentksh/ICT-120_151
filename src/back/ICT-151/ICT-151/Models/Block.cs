using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    public class Block
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The user blocking another user
        /// </summary>
        public User Blocker { get; set; }

        /// <summary>
        /// The user blocking another user
        /// </summary>
        [ForeignKey(nameof(Blocker))]
        [Required]
        public Guid BlockerId { get; set; }

        /// <summary>
        /// The user being blocked
        /// </summary>
        public User BlockTarget { get; set; }

        /// <summary>
        /// The user being blocked
        /// </summary>
        [ForeignKey(nameof(BlockTarget))]
        [Required]
        public Guid BlockTargetId { get; set; }
    }
}
