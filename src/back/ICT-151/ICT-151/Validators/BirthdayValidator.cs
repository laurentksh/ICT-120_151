using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Validators
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class BirthdayValidator : ValidationAttribute
    {
        /// <summary>
        /// Minimum age required to be validated. (-1 to ignore, default: 13)
        /// </summary>
        public int MinimumAgeRequired { get; set; } = 13;

        /// <summary>
        /// Maximum age required to be validated. (-1 to ignore, default: 120)
        /// </summary>
        public int MaximumAgeRequired { get; set; } = 120;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
                return ValidationResult.Success;

            var date = (DateTime)value;

            if (date > DateTime.Today)
                return new ValidationResult("Date greater than today.");
            
            if (MinimumAgeRequired != -1) {
                if ((DateTime.Today - date).Days < (MinimumAgeRequired * 365.25f)) {
                    return new ValidationResult($"You must be {MinimumAgeRequired} year(s) old to use this service/feature.");
                }
            }

            if (MaximumAgeRequired != -1) {
                if ((DateTime.Today - date).Days > (MaximumAgeRequired * 365.25f))
                    return new ValidationResult("Invalid birthday date.");
            }

            return ValidationResult.Success;
        }
    }
}
