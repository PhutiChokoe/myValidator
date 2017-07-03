using MyValidatorService.Models;
using NUnit.Framework;

namespace MyValidatorService.Tests.Models
{
    [TestFixture]
    class IdValidatorTest
    {
        private IdValidator _IdValidator;
        private const string _CorrectId = "9111295839084";
        public IdValidatorTest()
        {
            Setup();
        }
        [SetUp]
        public void Setup()
        {
            _IdValidator = new IdValidator();
        }
        [Test]
        public void IsIdLengthValid()
        {
            var result = _IdValidator.IsIdLengthValid(_CorrectId);
            Assert.IsTrue(result);
        }
        [Test]
        public void IsIdLengthInValid()
        {
            var result = _IdValidator.IsIdLengthValid("91113616");
            Assert.IsFalse(result);
        }
        [Test]
        public void IsCitizenshipValid()
        {
            var result = _IdValidator.IsCitizenshipValid("0");
            Assert.IsTrue(result);
        }
        [Test]
        public void IsCitizenshipInValid()
        {
            var result = _IdValidator.IsCitizenshipValid("3");
            Assert.IsFalse(result);
        }
        [Test]
        public void IsDateFormatValid()
        {
            var result = _IdValidator.IsDateFormatValid("911129");
            Assert.IsTrue(result);
        }
        [Test]
        public void IsDateFormatInValid()
        {
            var result = _IdValidator.IsDateFormatValid("911139");
            Assert.IsFalse(result);
        }
        [Test]
        public void IsGenderTypeCorrect()
        {
            var result = _IdValidator.IsGenderTypeCorrect("5839");
            Assert.IsTrue(result);
        }
        [Test]
        public void IsGenderTypeInCorrect()
        {
            var result = _IdValidator.IsGenderTypeCorrect("58319");
            Assert.IsFalse(result);
        }
        [Test]
        public void IsSequenceCorrect()
        {
            var result = _IdValidator.IsSequenceCorrect(_CorrectId);
            Assert.IsTrue(result);
        }
        [Test]
        public void ControlDigitValid()
        {
            var result = _IdValidator.ControlDigitTest(_CorrectId);
            Assert.IsTrue(result);
        }
        [Test]
        public void ControlDigitinValid()
        {
            var result = _IdValidator.ControlDigitTest("9111295839087");
            Assert.IsFalse(result);
        }
    }
}
