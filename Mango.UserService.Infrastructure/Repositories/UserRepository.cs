using System.Data;
using Dapper;
using Mango.UserService.Application.Interfaces;
using Mango.UserService.Domain.Entities;

namespace Mango.UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
  private readonly IDbConnection _db;

  public UserRepository(IDbConnection db)
  {
    _db = db;
  }

  public async Task<bool> EmailExistsAsync(string email)
  {
    const string query = "SELECT 1 FROM users WHERE email = @Email LIMIT 1";
    var result = await _db.QueryFirstOrDefaultAsync<int?>(query, new { Email = email });
    return result.HasValue;
  }

  public async Task AddAsync(User user)
  {
    const string query = @"
            INSERT INTO users (userid, firstname, lastname, email, passwordhash, role, createdat)
            VALUES (@UserId, @FirstName, @LastName, @Email, @PasswordHash, @Role, @CreatedAt)";

    await _db.ExecuteAsync(query, user);
  }
}
