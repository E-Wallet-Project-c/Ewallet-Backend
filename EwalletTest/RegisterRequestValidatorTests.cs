using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Validators;
using FluentValidation.TestHelper;

namespace TestEwallet
{
    public class RegisterRequestValidatorTests
    {
        private readonly RegisterRequestValidator _validator;
        public RegisterRequestValidatorTests()
        {
            _validator = new RegisterRequestValidator();
        }





        // FULL NAME TESTS
        [Theory]
        [InlineData("")]
        [InlineData("Mo")]
        public void Should_Have_Error_When_FullName_Is_Invalid(string name)
        {
            var model = new UserRegisterRequest { FullName = name };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FullName);
        }

        [Fact]
        public void Should_Not_Have_Error_When_FullName_Is_Valid()
        {
            var model = new UserRegisterRequest { FullName = "Mohammed Abu Abdo" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.FullName);
        }

        //  EMAIL TESTS
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("plainaddress")]
        [InlineData("@missingusername.com")]
        [InlineData("username@.com")]
        [InlineData("username@com")]
        [InlineData("username@domain..com")]
        public void Should_Have_Error_When_Email_Is_Invalid(string email)
        {
            var model = new UserRegisterRequest { Email = email };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("test@gmail.com")]
        [InlineData("user@yahoo.com")]
        [InlineData("hello@outlook.com")]
        public void Should_Not_Have_Error_When_Email_Is_Valid(string email)
        {
            var model = new UserRegisterRequest { Email = email };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
        }

        ////  DATE OF BIRTH TESTS
        //[Fact]
        //public void Should_Have_Error_When_DateOfBirth_Is_InFuture()
        //{
        //    var model = new UserRegisterRequest { DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1)) };
        //    var result = _validator.TestValidate(model);
        //    result.ShouldHaveValidationErrorFor(x => x.DateOfBirth);
        //}

        //[Fact]
        //public void Should_Not_Have_Error_When_DateOfBirth_Is_Valid()
        //{
        //    var model = new UserRegisterRequest { DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20)) };
        //    var result = _validator.TestValidate(model);
        //    result.ShouldNotHaveValidationErrorFor(x => x.DateOfBirth);
        //}

        //  PHONE TESTS
        //[Theory]
        //[InlineData("")]
        //[InlineData("0712345678")]
        //[InlineData("079123456")]
        //public void Should_Have_Error_When_Phone_Is_Invalid(string phone)
        //{
        //    var model = new UserRegisterRequest { Phone = phone };
        //    var result = _validator.TestValidate(model);
        //    result.ShouldHaveValidationErrorFor(x => x.Phone);
        //}

        //[Theory]
        //[InlineData("0791234567")]
        //[InlineData("0789999999")]
        //[InlineData("0770000000")]
        //public void Should_Not_Have_Error_When_Phone_Is_Valid(string phone)
        //{
        //    var model = new UserRegisterRequest { Phone = phone };
        //    var result = _validator.TestValidate(model);
        //    result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        //}

        //  PASSWORD TESTS
        [Theory]
        [InlineData("")]
        [InlineData("short")]
        [InlineData("alllowercase1!")]
        [InlineData("ALLUPPERCASE1!")]
        [InlineData("NoDigits!!")]
        public void Should_Have_Error_When_Password_Is_Invalid(string password)
        {
            var model = new UserRegisterRequest { Password = password };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_Password_Is_Valid()
        {
            var model = new UserRegisterRequest { Password = "StrongPass1!" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }

        //  CONFIRM PASSWORD TESTS
        [Fact]
        public void Should_Have_Error_When_ConfirmPassword_DoesNotMatch()
        {
            var model = new UserRegisterRequest { Password = "StrongPass1!", ConfirmPassword = "DifferentPass1!" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.ConfirmPassword);
        }

        [Fact]
        public void Should_Not_Have_Error_When_ConfirmPassword_Matches()
        {
            var model = new UserRegisterRequest { Password = "StrongPass1!", ConfirmPassword = "StrongPass1!" };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword);
        }
    }
}   

