using Dapper;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;
using System.Data;

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
            var getAllQuery = @"SELECT id AS Id, user_id AS UserId, movie_id AS MovieId, 
                                       review AS Review, grade AS Grade, created_at AS CreatedAt
                                FROM ratings ";
            return await connection.QueryAsync<Rating>(getAllQuery);
        }

        public async Task<Rating?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS Id, user_id AS UserId, movie_id AS MovieId, 
                                       review AS Review, grade AS Grade, created_at AS CreatedAt
                                FROM ratings
                                WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Rating>(getByIdQuery, new { Id = id });
        }

        public async Task<int> Add(Rating newComment)
        {
            var addCommentQuery = @"INSERT INTO ratings(user_id, movie_id, grade, review) 
                                        VALUES (@UserId, @MovieId, @Grade, @Review)
                                        RETURNING id AS Id";
            return await connection.ExecuteScalarAsync<int>(addCommentQuery, newComment);
        }

        public async Task<bool> Update(Rating newComment)
        {
            var updateCommentQuery = @"UPDATE ratings SET grade = @Grade, review = @Review 
                                       WHERE id = @Id";

            return await connection.ExecuteAsync(updateCommentQuery, newComment) > 0;
        }
        public async Task<bool> Delete(int id)
        {
            Console.WriteLine(id);
            var deleteCommentQuery = "DELETE FROM ratings WHERE id = @Id";

            return await connection.ExecuteAsync(deleteCommentQuery, new { Id = id }) > 0;
        }

        public async Task<IEnumerable<Rating>> GetFiltered(string filter, string parameter)
        {
            var getFilteredMoviesQuery = $@"SELECT id AS Id, user_id AS UserId, movie_id AS MovieId, 
                                            review AS Review, grade AS Grade, created_at AS CreatedAt
                                            FROM ratings 
                                            WHERE {filter} = '{parameter}'";
            return await connection.QueryAsync<Rating>(getFilteredMoviesQuery);
        }
    }
}
