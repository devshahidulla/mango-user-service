namespace Mango.UserService.Application.DTOs;

/// <summary>
/// User response DTO returned after registration or user queries
/// </summary>
public class UserResponse
{
  public Guid UserId { get; set; }
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public string FullName { get; set; } = default!;
  public string Email { get; set; } = default!;
  public string Role { get; set; } = default!;
  public DateTime CreatedAt { get; set; }
}
