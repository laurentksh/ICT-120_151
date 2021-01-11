using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ICT_151.Models;
using ICT_151.Validators;
using ICT_151.Models.Dto;

namespace ICT_151_Tests.Validators
{
    [TestClass]
    public class BirthdayValidatorTests
    {
        [TestMethod]
        public void MinAgeRequirementTest()
        {
            var birthdayValidator = new BirthdayValidator();
            birthdayValidator.MinimumAgeRequired = 13;

            var birthDay1 = DateTime.Today.AddYears(-9);
            var birthDay2 = DateTime.Today.AddYears(-16);

            var result1 = birthdayValidator.IsValid(birthDay1);
            var result2 = birthdayValidator.IsValid(birthDay2);

            Assert.IsFalse(result1);
            Assert.IsTrue(result2);
        }

        [TestMethod]
        public void MaxAgeRequirementTest()
        {
            var birthdayValidator = new BirthdayValidator();
            birthdayValidator.MaximumAgeRequired = 99;

            var birthDay1 = DateTime.Today.AddYears(-98);
            var birthDay2 = DateTime.Today.AddYears(-100);

            var result1 = birthdayValidator.IsValid(birthDay1);
            var result2 = birthdayValidator.IsValid(birthDay2);

            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [TestMethod]
        public void DateValidityTest()
        {
            var birthday = DateTime.Today.AddDays(2);

            var result = new BirthdayValidator().IsValid(birthday);

            Assert.IsFalse(result);
        }
    }
}
