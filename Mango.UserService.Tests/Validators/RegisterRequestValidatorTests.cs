using FluentValidation.TestHelper;
using Mango.UserService.Application.DTOs;
using Mango.UserService.Application.Validators;

namespace Mango.UserService.Tests.Validators;

public class RegisterRequestValidatorTests
{
  private readonly RegisterRequestValidator _validator;

  public RegisterRequestValidatorTests()
  {
    _validator = new RegisterRequestValidator();
  }

  [Fact]
  public void Should_Have_Error_When_FirstName_Is_Empty()
  {
    var model = new RegisterRequest { FirstName = "" };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.FirstName);
  }

  [Fact]
  public void Should_Have_Error_When_FirstName_Exceeds_MaxLength()
  {
    var model = new RegisterRequest { FirstName = new string('a', 51) };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.FirstName);
  }

  [Fact]
  public void Should_Have_Error_When_LastName_Is_Empty()
  {
    var model = new RegisterRequest { LastName = "" };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.LastName);
  }

  [Fact]
  public void Should_Have_Error_When_LastName_Exceeds_MaxLength()
  {
    var model = new RegisterRequest { LastName = new string('a', 51) };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.LastName);
  }

  [Fact]
  public void Should_Have_Error_When_Email_Is_Empty()
  {
    var model = new RegisterRequest { Email = "" };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Email);
  }

  [Fact]
  public void Should_Have_Error_When_Email_Is_Invalid()
  {
    var model = new RegisterRequest { Email = "invalid-email" };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Email);
  }

  [Fact]
  public void Should_Have_Error_When_Password_Is_Empty()
  {
    var model = new RegisterRequest { Password = "" };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Should_Have_Error_When_Password_Is_Too_Short()
  {
    var model = new RegisterRequest { Password = "short" };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Password);
  }

  [Fact]
  public void Should_Not_Have_Error_When_Role_Is_Null()
  {
    var model = new RegisterRequest
    {
      FirstName = "John",
      LastName = "Doe",
      Email = "john@example.com",
      Password = "SecurePass123",
      Role = null
    };
    var result = _validator.TestValidate(model);
    result.ShouldNotHaveValidationErrorFor(x => x.Role);
  }

  [Fact]
  public void Should_Not_Have_Error_When_Role_Is_Valid()
  {
    var validRoles = new[] { "Farmer", "Reseller", "Wholesaler" };
    foreach (var role in validRoles)
    {
      var model = new RegisterRequest
      {
        FirstName = "John",
        LastName = "Doe",
        Email = "john@example.com",
        Password = "SecurePass123",
        Role = role
      };
      var result = _validator.TestValidate(model);
      result.ShouldNotHaveValidationErrorFor(x => x.Role);
    }
  }

  [Fact]
  public void Should_Have_Error_When_Role_Is_Invalid()
  {
    var model = new RegisterRequest
    {
      FirstName = "John",
      LastName = "Doe",
      Email = "john@example.com",
      Password = "SecurePass123",
      Role = "InvalidRole"
    };
    var result = _validator.TestValidate(model);
    result.ShouldHaveValidationErrorFor(x => x.Role);
  }

  [Fact]
  public void Should_Pass_Validation_With_Valid_Request_Without_Role()
  {
    var model = new RegisterRequest
    {
      FirstName = "John",
      LastName = "Doe",
      Email = "john.doe@example.com",
      Password = "SecurePassword123"
    };
    var result = _validator.TestValidate(model);
    result.ShouldNotHaveAnyValidationErrors();
  }

  [Fact]
  public void Should_Pass_Validation_With_Valid_Request_With_Role()
  {
    var model = new RegisterRequest
    {
      FirstName = "Jane",
      LastName = "Smith",
      Email = "jane.smith@example.com",
      Password = "SecurePassword456",
      Role = "Farmer"
    };
    var result = _validator.TestValidate(model);
    result.ShouldNotHaveAnyValidationErrors();
  }
}
