namespace Mango.UserService.Application.DTOs;

/// <summary>
/// Registration request DTO matching the frontend registration form fields.
/// Frontend fields: firstName, lastName, email, password (confirmPassword is validated client-side only)
/// Role is optional and defaults to "Farmer" if not provided, as the UI doesn't collect role during registration.
/// </summary>
public class RegisterRequest
{
  /// <summary>
  /// User's first name
  /// </summary>
  public string FirstName { get; set; } = default!;
  
  /// <summary>
  /// User's last name
  /// </summary>
  public string LastName { get; set; } = default!;
  
  /// <summary>
  /// User's email address
  /// </summary>
  public string Email { get; set; } = default!;
  
  /// <summary>
  /// User's password (will be hashed before storage)
  /// </summary>
  public string Password { get; set; } = default!;
  
  /// <summary>
  /// User's role (Farmer, Reseller, or Wholesaler). Defaults to "Farmer" if not provided.
  /// </summary>
  public string? Role { get; set; }
}
