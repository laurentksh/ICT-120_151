using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ICT_151.Validators;
using Microsoft.AspNetCore.Identity;

namespace ICT_151.Models
{
    public class User //I was going to use IdentityUser<Guid> in the first place but I realised it was too much work implementing it.
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Username, can only contain "a-zA-Z0-9_-".
        /// </summary>
        [Required, StringLength(24, MinimumLength = 2), RegularExpression("[a-zA-Z0-9-_]*\\w", MatchTimeoutInMilliseconds = 1000)]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// SHA-512 Hash
        /// </summary>
        [Required]
        public string PasswordHash { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public string Biography { get; set; }

        [Required, BirthdayValidator(MinimumAgeRequired = 13)]
        public DateTime Birthday { get; set; }

        [Required]
        public AccountType AccountType { get; set; } = AccountType.User;

        public AccountDeactivationType AccountDeactivationType { get; set; } = AccountDeactivationType.None;

        [Required]
        public DateTime CreationDate { get; set; }

        public string ProfilePictureUrl { get; set; } = "/api/Media/image/default_pp";


        public List<UserSession> UserSessions { get; set; }

        //public List<Publication> Publications { get; set; } //Can't be used since publications also contains replies

        [InverseProperty(nameof(Repost.User))]
        public List<Repost> Reposts { get; set; }

        [InverseProperty(nameof(Like.User))]
        public List<Like> Likes { get; set; }

        [InverseProperty(nameof(Block.Blocker))]
        public List<Block> Blocking { get; set; }

        [InverseProperty(nameof(Block.BlockTarget))]
        public List<Block> Blocked { get; set; }

        [InverseProperty(nameof(Follow.Follower))]
        public List<Follow> Following { get; set; }

        [InverseProperty(nameof(Follow.FollowTarget))]
        public List<Follow> Followed { get; set; }

        [InverseProperty(nameof(PrivateMessage.Recipient))]
        public List<PrivateMessage> Sending { get; set; }

        [InverseProperty(nameof(PrivateMessage.Sender))]
        public List<PrivateMessage> Receiving { get; set; }
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

        [Required]
        public string Token { get; set; }

        public IPAddress RemoteHost { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime ExpiracyDate { get; set; }


        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
