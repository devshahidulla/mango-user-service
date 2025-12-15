using Mango.UserService.Domain.Enums;

namespace Mango.UserService.Domain.Entities;

public class User
{
  public Guid UserId { get; set; }
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public string Email { get; set; } = default!;
  public string PasswordHash { get; set; } = default!;
  public UserRole Role { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  
  /// <summary>
  /// Computed property for full name (FirstName + LastName)
  /// </summary>
  public string FullName => $"{FirstName} {LastName}";
}
