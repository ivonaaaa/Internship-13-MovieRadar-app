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

        public async Task<(int, int)> GetLikesAndDislikes(int ratingId)
        {
            var likesAndDislikesQuery = @$"SELECT SUM(CASE WHEN reaction = 'like' THEN 1 ELSE 0 END) AS Likes,
                                                  SUM(CASE WHEN reaction = 'dislike' THEN 1 ELSE 0 END) AS Dislikes
                                           FROM ratings_reactions 
                                           WHERE rating_id = @Id";
            return await connection.QuerySingleAsync<(int, int)>(likesAndDislikesQuery, new { Id = ratingId });
        }

        public async Task<bool> RemoveLikeDislike(int reactionId)
        {
            var removeLikeDislikeQuery = @"DELETE FROM ratings_reactions 
                                           WHERE id = @Id";
            return await connection.ExecuteAsync(removeLikeDislikeQuery, new { Id = reactionId}) > 0;
        }
    }
}
