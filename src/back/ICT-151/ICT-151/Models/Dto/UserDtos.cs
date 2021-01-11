using System;
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
        /// false: Default token validity  (7 days)
        /// true : Extended token validity (30 days)
        /// </summary>
        public bool ExtendSession { get; set; } = false;
    }

    public class CreateUserDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(24, MinimumLength = 4)]
        public string Username { get; set; }

        [Required, StringLength(128, MinimumLength = 6)]
        public string Password { get; set; }

        [Required, BirthdayValidator(MinimumAgeRequired = 13)]
        public DateTime BirthDay { get; set; }


    }
}
