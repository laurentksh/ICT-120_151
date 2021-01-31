using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    public class Block
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The user blocking another user
        /// </summary>
        public User Blocker { get; set; }

        /// <summary>
        /// The user blocking another user
        /// </summary>
        public Guid BlockerId { get; set; }

        /// <summary>
        /// The user being blocked
        /// </summary>
        public User BlockTarget { get; set; }

        /// <summary>
        /// The user being blocked
        /// </summary>
        public Guid BlockTargetId { get; set; }
    }
}
