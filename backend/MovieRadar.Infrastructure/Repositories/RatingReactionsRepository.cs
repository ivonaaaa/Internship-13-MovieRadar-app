using Dapper;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;
using System.Data;


namespace MovieRadar.Infrastructure.Repositories
{
    public class RatingReactionsRepository : IRatingReactionsRepository
    {
        private readonly IDbConnection connection;
        public RatingReactionsRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<RatingsReactions>> GetAll()
        {
            var getAllReactionsQuery = @"SELECT id AS Id, rating_id AS RatingId, reaction AS Reaction, user_id AS UserId
                                         FROM ratings_reactions";
            return await connection.QueryAsync<RatingsReactions>(getAllReactionsQuery);
        }

        public async Task<RatingsReactions?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS Id, rating_id AS RatingId, reaction AS Reaction, user_id AS UserId
                                 FROM ratings_reactions
                                 WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<RatingsReactions>(getByIdQuery, new { Id = id });
        }

        public async Task<int> Add(RatingsReactions newReaction)
        {
            var reactionQuery = @"INSERT INTO ratings_reactions(rating_id, reaction, user_id)
                                  VALUES (@RatingId, CAST(@Reaction AS reaction_type), @UserId)
                                  RETURNING id";

            return await connection.ExecuteScalarAsync<int>(reactionQuery, new { RatingId = newReaction.RatingId, Reaction = newReaction.Reaction.ToString().ToLower(), UserId = newReaction.UserId });
        }

        public async Task<bool> Update(RatingsReactions updatedReactions)
        {
            var updateReaction = @"UPDATE ratings_reactions 
                                   SET rating_id = @RatingId, reaction = CAST(@Reaction AS reaction_type), user_id = @UserId
                                   WHERE id = @Id";
            return await connection.ExecuteAsync(updateReaction, new
            {
                updatedReactions.RatingId,
                Reaction = updatedReactions.Reaction.ToString().ToLower(),
                UserId = updatedReactions.UserId,
                updatedReactions.Id
            }) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteReactionQuery = "DELETE FROM ratings_reactions WHERE id = @Id";
            return await connection.ExecuteAsync(deleteReactionQuery, new { Id = id }) > 0;
        }

        public async Task<IEnumerable<RatingsReactions>> GetAllByRatingId(int id)
        {
            var allReactionsByRatingId = @"SELECT id AS Id, rating_id AS RatingId, reaction AS Reaction, user_id AS UserId
                                           FROM ratings_reactions
                                           WHERE rating_id = @Id";
            return await connection.QueryAsync<RatingsReactions>(allReactionsByRatingId, new { Id = id });
        }
    }
}
