using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Models
{
    public class Reply
    {
        public Guid Id { get; set; }

        [Required, StringLength(280, MinimumLength = 1)]
        public string TextContent { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }


        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required]
        public Guid SubmissionId { get; set; }

        public Submission Submission { get; set; }
    }
}
