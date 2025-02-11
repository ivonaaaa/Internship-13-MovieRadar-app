
using System.Data;
using Dapper;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Infrastructure.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var getQuery = "SELECT * FROM users";
            return await _connection.QueryAsync<User>(getQuery);
        }

        public async Task<User?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS UserId, first_name AS FirstName, last_name AS LastName, email AS Email, password AS Password,
                is_admin AS IsAdmin FROM users WHERE id = @UserId";
            return await _connection.QuerySingleOrDefaultAsync<User>(getByIdQuery, new { UserId = id });
        }

        public async Task<int> Add(User newUser)
        {
            var addUserQuery = @"INSERT INTO users(first_name, last_name, email, password) VALUES (@FirstName, @LastName, @Email, @Password) 
                RETURNING first_name AS FirstName, last_name AS LastName, email AS Email, password AS Password";
            return await _connection.ExecuteScalarAsync<int>(addUserQuery, newUser);
        }

        public async Task<bool> Update(User user)
        {
            var updateUserQuery = "UPDATE users SET first_name = @FirstName, last_name = @LastName, email = @Email, password = @Password WHERE id = @UserId";
            int rows = await _connection.ExecuteAsync(updateUserQuery, user);

            return rows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteQuery = "DELETE FROM users WHERE id = @UserId";
            int rows = await _connection.ExecuteAsync(deleteQuery, new { UserId = id });

            return rows > 0;
        }
    }
}
