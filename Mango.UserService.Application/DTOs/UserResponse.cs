namespace Mango.UserService.Application.DTOs;

public class UserResponse
{
  public Guid UserId { get; set; }
  public string FullName { get; set; } = default!;
  public string Email { get; set; } = default!;
  public string Role { get; set; } = default!;
  public DateTime CreatedAt { get; set; }
}
