using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Validators;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EwalletTest
{
    public class UserProfileRequestValidatorTests
    {
        private readonly UserProfileRequestValidator _validator;

        public UserProfileRequestValidatorTests()
        {
            _validator = new UserProfileRequestValidator();
        }

        [Theory]
        [InlineData("0771234567")]
        [InlineData("0781234567")]
        [InlineData("0791234567")]
        public void Phone_ShouldNotHaveValidationError_WhenFormatIsCorrect(string validPhone)
        {
            // Arrange
            var request = new UserProfileRequest { Phone = validPhone };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Theory]
        [InlineData("0761234567")] // Invalid prefix
        [InlineData("077123456")]  // Too short
        [InlineData("07812345678")] // Too long
        [InlineData(" ")]           // Whitespace
        [InlineData(null)]          // Null
        [InlineData("")]            // Empty
        public void Phone_ShouldHaveValidationError_WhenFormatIsIncorrect(string invalidPhone)
        {
            // Arrange
            var request = new UserProfileRequest { Phone = invalidPhone };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [Fact]
        public void DateOfBirth_ShouldNotHaveValidationError_WhenDateIsValidAndInThePast()
        {
            // Arrange
            var request = new UserProfileRequest { DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)) };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Fact]
        public void DateOfBirth_ShouldHaveValidationError_WhenDateIsInTheFuture()
        {
            // Arrange
            var request = new UserProfileRequest { DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1)) };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth)
                  .WithErrorMessage("Date of birth must be a valid date and cannot be in the future.");
        }

        [Fact]
        public void DateOfBirth_ShouldHaveValidationError_WhenDateIsNull()
        {
            // Arrange
            var request = new UserProfileRequest { DateOfBirth = null };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        }

        [Theory]
        [InlineData("Jordan")]
        [InlineData("Saudi Arabia")]
        public void Country_ShouldNotHaveValidationError_WhenCountryIsInTheList(string validCountry)
        {
            // Arrange
            var request = new UserProfileRequest { Country = validCountry };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Country);
        }

        [Theory]
        [InlineData("USA")]
        [InlineData("Germany")]
        [InlineData(null)]
        [InlineData("")]
        public void Country_ShouldHaveValidationError_WhenCountryIsNotInTheListOrEmpty(string invalidCountry)
        {
            // Arrange
            var request = new UserProfileRequest { Country = invalidCountry };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Country);
        }
    }

}

