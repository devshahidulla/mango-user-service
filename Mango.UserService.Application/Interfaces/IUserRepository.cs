using Mango.UserService.Domain.Entities;

namespace Mango.UserService.Application.Interfaces;

public interface IUserRepository
{
  Task<bool> EmailExistsAsync(string email);
  Task AddAsync(User user);
}
