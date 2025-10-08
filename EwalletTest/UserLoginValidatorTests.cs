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
    public class UserLoginValidatorTests
    {
        private readonly UserLoginValidator _validator;

        public UserLoginValidatorTests()
        {
            _validator = new UserLoginValidator();
        }

        #region Password Tests

        [Fact]
        public void Password_ShouldNotHaveValidationError_WhenPasswordIsValid()
        {
            // Arrange
            var request = new UserLoginRequest { Password = "Password123!" };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("short")] // Too short
        [InlineData("thispasswordistoolongandwillfail")] // Too long
        [InlineData("password123!")] // Missing uppercase
        [InlineData("PASSWORD123!")] // Missing lowercase
        [InlineData("Password!")]    // Missing digit
        [InlineData("Password123")]  // Missing special character
        public void Password_ShouldHaveValidationError_WhenPasswordIsInvalid(string invalidPassword)
        {
            // Arrange
            var request = new UserLoginRequest { Password = invalidPassword };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        #endregion

        #region Email Tests

        [Theory]
        [InlineData("test@gmail.com")]
        [InlineData("test.user@yahoo.com")]
        [InlineData("another_user@outlook.com")]
        public void Email_ShouldNotHaveValidationError_WhenEmailIsValid(string validEmail)
        {
            // Arrange
            var request = new UserLoginRequest { Email = validEmail };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("invalid-email")] // Not a valid email format
        [InlineData("test@domain.com")] // Not a whitelisted domain
        [InlineData("test@gmail.co")] // Invalid domain suffix
        [InlineData("this.is.a.very.long.email.address.that.will.fail@gmail.com")] // Too long
        public void Email_ShouldHaveValidationError_WhenEmailIsInvalid(string invalidEmail)
        {
            // Arrange
            var request = new UserLoginRequest { Email = invalidEmail };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Email_ShouldHaveValidationError_WhenEmailIsFromDisallowedDomain()
        {
            // Arrange
            var request = new UserLoginRequest { Email = "user@company.net" };

            // Act
            var result = _validator.TestValidate(request);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("Email must be valid and end with gmail.com, yahoo.com, or outlook.com.");
        }

        #endregion
    }

}

