using Mango.UserService.Domain.Entities;
using Mango.UserService.Domain.Enums;

namespace Mango.UserService.Tests.Domain;

public class UserTests
{
  [Fact]
  public void FullName_Should_Concatenate_FirstName_And_LastName()
  {
    var user = new User
    {
      FirstName = "John",
      LastName = "Doe"
    };

    Assert.Equal("John Doe", user.FullName);
  }

  [Fact]
  public void FullName_Should_Handle_Only_FirstName()
  {
    var user = new User
    {
      FirstName = "John",
      LastName = ""
    };

    Assert.Equal("John", user.FullName);
  }

  [Fact]
  public void FullName_Should_Handle_Only_LastName()
  {
    var user = new User
    {
      FirstName = "",
      LastName = "Doe"
    };

    Assert.Equal("Doe", user.FullName);
  }

  [Fact]
  public void FullName_Should_Trim_Whitespace()
  {
    var user = new User
    {
      FirstName = "  John  ",
      LastName = "  Doe  "
    };

    Assert.Equal("John Doe", user.FullName);
  }

  [Fact]
  public void FullName_Should_Handle_Null_Values()
  {
    var user = new User
    {
      FirstName = null!,
      LastName = "Doe"
    };

    Assert.Equal("Doe", user.FullName);
  }

  [Fact]
  public void User_Should_Have_Correct_Default_CreatedAt()
  {
    var beforeCreate = DateTime.UtcNow;
    var user = new User
    {
      UserId = Guid.NewGuid(),
      FirstName = "John",
      LastName = "Doe",
      Email = "john@example.com",
      PasswordHash = "hashedpassword",
      Role = UserRole.Farmer
    };
    var afterCreate = DateTime.UtcNow;

    Assert.True(user.CreatedAt >= beforeCreate && user.CreatedAt <= afterCreate);
  }
}
