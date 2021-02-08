using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ICT_151.Validators;

namespace ICT_151.Models.Dto
{
    public class AuthUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        /// <summary>
        /// false: Default token validity  (4 hours)
        /// true : Extended token validity (30 days)
        /// </summary>
        public bool ExtendSession { get; set; } = false;
    }

    public class CreateUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(24, MinimumLength = 2), RegularExpression("[a-zA-Z0-9-_]*\\w", MatchTimeoutInMilliseconds = 1000)]
        public string Username { get; set; }

        [Required, StringLength(128, MinimumLength = 6)]
        public string Password { get; set; }

        [Required, BirthdayValidator(MinimumAgeRequired = 13)]
        public DateTime BirthDay { get; set; }
    }

    public class UpdateUserDto
    {
        [StringLength(128, MinimumLength = 6)]
        public string Password { get; set; }

        [StringLength(128, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(24, MinimumLength = 2), RegularExpression("[a-zA-Z0-9-_]*\\w", MatchTimeoutInMilliseconds = 1000)]
        public string Username { get; set; }

        [StringLength(200, MinimumLength = 0)]
        public string Biography { get; set; }

        [BirthdayValidator(MinimumAgeRequired = 13)]
        public DateTime? BirthDay { get; set; }
    }

    public class UserSummaryViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string ProfilePictureId { get; set; }

        public string Biography { get; set; }

        public DateTime Birthday { get; set; }

        public DateTime CreationDateUtc { get; set; }

        public bool Following { get; set; }

        public bool FollowsMe { get; set; }

        public bool Blocking { get; set; }

        public bool BlocksMe { get; set; }

        public static UserSummaryViewModel FromUser(User user, Guid? userId = null) => new UserSummaryViewModel
        {
            Id = user.Id,
            Username = user.Username,
            ProfilePictureId = user.ProfilePictureMediaId == null ? "default_pp" : user.ProfilePictureMediaId.ToString(),
            Biography = user.Biography,
            Birthday = user.Birthday,
            CreationDateUtc = user.CreationDate,
            Following = userId.HasValue && user.Followed.Any(x => x.FollowerId == userId),
            FollowsMe = userId.HasValue && user.Following.Any(x => x.FollowTargetId == userId),
            Blocking = userId.HasValue && user.Blocked.Any(x => x.BlockerId == userId),
            BlocksMe = userId.HasValue && user.Blocking.Any(x => x.BlockTargetId == userId)
        };
    }

    public class CreatedUserViewModel
    {
        public UserSummaryViewModel User { get; set; }

        public UserSessionViewModel Session { get; set; }
    }

    public class UserSessionViewModel
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime CreationDateUtc { get; set; }

        public DateTime ExpiracyDateUtc { get; set; }

        public Guid UserId { get; set; }

        public static UserSessionViewModel FromUserSession(UserSession session) => new UserSessionViewModel
        {
            Id = session.Id,
            Token = session.Token,
            CreationDateUtc = session.CreationDate,
            ExpiracyDateUtc = session.ExpiracyDate,
            UserId = session.UserId
        };
    }

    public class UserSessionSummaryViewModel
    {
        public Guid Id { get; set; }

        public IPAddress RemoteHost { get; set; }

        public DateTime CreationDateUtc { get; set; }

        public DateTime ExpiracyDateUtc { get; set; }

        public static UserSessionSummaryViewModel FromUserSession(UserSession session) => new UserSessionSummaryViewModel
        {
            Id = session.Id,
            RemoteHost = session.RemoteHost,
            CreationDateUtc = session.CreationDate,
            ExpiracyDateUtc = session.ExpiracyDate
        };
    }
}
