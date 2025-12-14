using Mango.UserService.Domain.Enums;

namespace Mango.UserService.Domain.Entities;

public class User
{
  public Guid UserId { get; set; }
  public string FullName { get; set; } = default!;
  public string Email { get; set; } = default!;
  public string PasswordHash { get; set; } = default!;
  public UserRole Role { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
