﻿using System.Data;
using Dapper;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var getQuery = "SELECT id AS Id, first_name AS FirstName, last_name AS LastName, email AS Email, is_admin AS IsAdmin FROM users";
            return await _connection.QueryAsync<User>(getQuery);
        }

        public async Task<User?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS Id, first_name AS FirstName, last_name AS LastName, email AS Email,
                is_admin AS IsAdmin FROM users WHERE id = @Id";
            return await _connection.QuerySingleOrDefaultAsync<User>(getByIdQuery, new { Id = id });
        }

        public async Task<int> Add(User newUser)
        {
            var addUserQuery = @"INSERT INTO users(first_name, last_name, email, password) VALUES (@FirstName, @LastName, @Email, @Password)
                RETURNING Id";
            return await _connection.QuerySingleAsync<int>(addUserQuery, newUser);
        }

        public async Task<bool> Update(User user)
        {
            var updateUserQuery = "UPDATE users SET first_name = @FirstName, last_name = @LastName, password = @Password WHERE id = @Id";
            int rows = await _connection.ExecuteAsync(updateUserQuery, user);

            return rows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteQuery = "DELETE FROM users WHERE id = @Id";
            int rows = await _connection.ExecuteAsync(deleteQuery, new { Id = id });

            return rows > 0;
        }

        public async Task<User?> GetByEmail(string email)
        {
            string query = @"SELECT id AS Id, first_name AS FirstName, last_name AS LastName, email AS Email, 
                   is_admin AS IsAdmin FROM users WHERE email = @Email";
            return await _connection.QueryFirstOrDefaultAsync<User>(query, new { Email = email });
        }

        public async Task<bool> CheckAuthData(string email, string password)
        {
            string checkAuthDataQuery  = @$"SELECT EXISTS( SELECT 1 
                                           FROM users
                                           WHERE email = @Email AND password = @Password)";
            return await _connection.ExecuteScalarAsync<bool>(checkAuthDataQuery, new { Email = email, Password = password });
            
        }
    }
}
