using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ICT_151.Models
{
    public class User : IdentityUser<Guid>
    {
        /// <summary>
        /// Username, can only contain "a-zA-Z0-9_-".
        /// </summary>
        [Required, StringLength(32, MinimumLength = 2), RegularExpression("[a-zA-Z0-9-_]*\\w", MatchTimeoutInMilliseconds = 1000)]
        public string Username { get; set; }
    }

    public class UserSession
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiracyDate { get; set; }
    }
}
