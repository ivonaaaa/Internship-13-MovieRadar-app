using Dapper;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

using System.Data;

namespace MovieRadar.Infrastructure.Repositories
{
    public class RatingCommentRepository : IRatingCommentRepository
    {
        private readonly IDbConnection connection;
        public RatingCommentRepository(IDbConnection connection)
        {
                this.connection = connection;
        }
        
        public async Task<IEnumerable<RatingComment>> GetAll()
        {
            var getAllCommentsOnRatignQuery = @"SELECT id AS Id, rating_id AS RatingId, user_id AS UserId, comment AS Comment
                                                FROM ratings_comments";
            return await connection.QueryAsync<RatingComment>(getAllCommentsOnRatignQuery);
        }
        
        public async Task<RatingComment?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS Id, rating_id AS RatingId, user_id AS UserId, comment AS Comment
                                 FROM ratings_comments
                                 WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<RatingComment>(getByIdQuery, new { Id = id});
        }

        public async Task<int> Add(RatingComment newRatingsComment)
        {
            var addRatingCommentQuery = @"INSERT INTO ratings_comments(rating_id, user_id, comment)
                                          VALUES (@RatingId, @UserId, @Comment)
                                          RETURNING id";
            return await connection.ExecuteScalarAsync<int>(addRatingCommentQuery, newRatingsComment);
        }

        public async Task<bool> Update(RatingComment ratingsComments)
        {
            var updateRatingComment = "UPDATE ratings_comments SET comment = @Comment WHERE id = @Id";
            return await connection.ExecuteAsync(updateRatingComment, ratingsComments) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteMovieQuery = "DELETE FROM ratings_comments WHERE id = @Id";
            return await connection.ExecuteAsync(deleteMovieQuery, new { Id = id }) > 0;
        }

        public async Task<IEnumerable<RatingComment>> GetFiltered(string filter, string value)
        {
            var getFilteredMoviesQuery = $@"SELECT id AS Id, rating_id AS RatingId, user_id AS UserId, comment AS Comment
                                            FROM ratings_comments
                                            WHERE {filter} = '{value}'";
            return await connection.QueryAsync<RatingComment>(getFilteredMoviesQuery);
        }
    }
}
