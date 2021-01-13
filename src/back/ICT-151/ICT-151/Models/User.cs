using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ICT_151.Models
{
    public class User //I was going to use IdentityUser<Guid> in the first place but I realised it was too much work implementing it.
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Username, can only contain "a-zA-Z0-9_-".
        /// </summary>
        [Required, StringLength(32, MinimumLength = 2), RegularExpression("[a-zA-Z0-9-_]*\\w", MatchTimeoutInMilliseconds = 1000)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// SHA-512 Hash
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public AccountType AccountType { get; set; } = AccountType.User;

        public AccountDeactivationType AccountDeactivationType { get; set; } = AccountDeactivationType.None;

        [Required]
        public DateTime CreationDate { get; set; }
    }

    public enum AccountType
    {
        /// <summary>
        /// Base account type
        /// </summary>
        User = 1,

        /// <summary>
        /// If selected, AccountDeactivationType must have a value.
        /// </summary>
        Disabled = -1,
        Administrator = 9
    }

    public enum AccountDeactivationType
    {
        None = 0,
        Unspecified = -1,
        Banned = 1,
        Disabled = 2,
        Deleted = 3,
    }

    public class UserSession
    {
        public static readonly TimeSpan DefaultTokenValidity = TimeSpan.FromHours(4);
        public static readonly TimeSpan ExtendedTokenValidity = TimeSpan.FromDays(30);

        public Guid Id { get; set; }

        public string Token { get; set; }

        public IPAddress RemoteHost { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiracyDate { get; set; }


        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
