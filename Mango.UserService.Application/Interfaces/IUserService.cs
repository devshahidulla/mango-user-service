using Mango.UserService.Application.DTOs;

namespace Mango.UserService.Application.Interfaces;

public interface IUserService
{
  Task<UserResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
}
