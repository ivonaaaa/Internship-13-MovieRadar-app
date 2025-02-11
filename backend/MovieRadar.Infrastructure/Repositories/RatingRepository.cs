using Dapper;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRadar.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly IDbConnection connection;
        public RatingRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Rating>> GetAll()
        {
            var getAllQuery = @"SELECT id AS RatingId, user_id AS UserId, movie_id AS MovieId, 
                                       review AS Review, grade AS Grade, created_at AS CreatedAt
                                FROM ratings ";
            return await connection.QueryAsync<Rating>(getAllQuery);
        }

        public async Task<Rating?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS RatingId, user_id AS UserId, movie_id AS MovieId, 
                                       review AS Review, grade AS Grade, created_at AS CreatedAt
                                FROM ratings
                            WHERE id = @RatingId";
            return await connection.QuerySingleOrDefaultAsync<Rating>(getByIdQuery, new { RatingId = id });
        }

        public async Task<int> Add(Rating newComment)
        {
            var addCommentQuery = @"INSERT INTO ratings(user_id, movie_id, grade, review) 
                                    VALUES (@UserId, @MovieId, @Grade, @Review)
                                    RETURNING id AS RatingId";
            return await connection.ExecuteScalarAsync<int>(addCommentQuery, newComment);
        }

        public async Task<bool> Update(Rating newComment)
        {
            var updateCommentQuery = "UPDATE ratings SET grade = @Grade, review = @Review";

            return await connection.ExecuteAsync(updateCommentQuery, newComment) > 0;
        }
        public async Task<bool> Delete(int id)
        {
            Console.WriteLine(id);
            var deleteCommentQuery = "DELETE FROM ratings WHERE id = @RatingId";

            return await connection.ExecuteAsync(deleteCommentQuery, new { RatingId = id }) > 0;
        }
    }
}
