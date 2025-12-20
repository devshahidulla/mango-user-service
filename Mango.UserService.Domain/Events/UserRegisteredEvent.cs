/// <summary>
/// Event published when a new user registers in the system
/// </summary>
public class UserRegisteredEvent
{
  public Guid UserId { get; set; }
  public string Email { get; set; } = default!;
  public string PasswordHash { get; set; } = default!;
  public string FirstName { get; set; } = default!;
  public string LastName { get; set; } = default!;
  public string FullName { get; set; } = default!;
  public string Role { get; set; } = default!;
  public DateTime CreatedAt { get; set; }
}
