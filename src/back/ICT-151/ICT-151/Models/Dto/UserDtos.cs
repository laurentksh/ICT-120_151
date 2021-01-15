﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

    public class UserSummaryViewModel
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string ProfilePictureUrl { get; set; }

        public DateTime CreationDate { get; set; }


        public static UserSummaryViewModel FromUser(User user) => new UserSummaryViewModel
        {
            Id = user.Id,
            Username = user.Username,
            ProfilePictureUrl = user.ProfilePictureUrl,
            CreationDate = user.CreationDate,
        };
    }

    public class UserSessionViewModel
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ExpiracyDate { get; set; }


        public Guid UserId { get; set; }
    }
}
