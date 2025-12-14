using Mango.UserService.Application.DTOs;
using Mango.UserService.Application.Interfaces;
using Mango.UserService.Domain.Entities;
using Mango.UserService.Domain.Enums;

namespace Mango.UserService.Application.Services;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;
  private readonly IEventPublisher _eventPublisher;

  public UserService(IUserRepository userRepository, IEventPublisher eventPublisher)
  {
    _userRepository = userRepository;
    _eventPublisher = eventPublisher;
  }

  public async Task<UserResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
  {
    // Check for duplicate emails in database  
    if (await _userRepository.EmailExistsAsync(request.Email))
      throw new Exception("Email already exists");

    // Hash the password (not done here for simplicity)  
    var user = new User
    {
      UserId = Guid.NewGuid(),
      FullName = request.FullName,
      Email = request.Email.ToLowerInvariant(),
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), // Simulated hash  
      Role = Enum.Parse<UserRole>(request.Role, ignoreCase: true),
      CreatedAt = DateTime.UtcNow
    };

    // Save user to DB via repository (infrastructure layer)  
    await _userRepository.AddAsync(user);

    // Publish user registered event (if using event-driven architecture)
    await _eventPublisher.PublishAsync("UserRegistered", "mango.user-service", new UserRegisteredEvent
    {
      UserId = user.UserId,
      Email = user.Email,
      PasswordHash = user.PasswordHash,
      FullName = user.FullName,
      Role = user.Role.ToString(),
      CreatedAt = user.CreatedAt
    }, cancellationToken);


    // Return DTO response  
    return new UserResponse
    {
      UserId = user.UserId,
      FullName = user.FullName,
      Email = user.Email,
      Role = user.Role.ToString(),
      CreatedAt = user.CreatedAt
    };
  }
}
