using Dapper;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;
using System.Data;

namespace MovieRadar.Infrastructure.Repositories
{
    public class RatingReactionRepository : IRatingReactionRepository
    {
        private readonly IDbConnection connection;
        public RatingReactionRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<RatingReaction>> GetAll()
        {
            var getAllReactionsQuery = @"SELECT id AS Id, rating_id AS RatingId, reaction AS Reaction, user_id AS UserId
                                         FROM ratings_reactions";
            return await connection.QueryAsync<RatingReaction>(getAllReactionsQuery);
        }

        public async Task<RatingReaction?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS Id, rating_id AS RatingId, reaction AS Reaction, user_id AS UserId
                                 FROM ratings_reactions
                                 WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<RatingReaction>(getByIdQuery, new { Id = id });
        }

        public async Task<int> Add(RatingReaction newReaction)
        {
            var reactionQuery = @"INSERT INTO ratings_reactions(rating_id, reaction, user_id)
                                  VALUES (@RatingId, CAST(@Reaction AS reaction_type), @UserId)
                                  RETURNING id";

            return await connection.ExecuteScalarAsync<int>(reactionQuery, new { RatingId = newReaction.RatingId, Reaction = newReaction.Reaction.ToString().ToLower(), UserId = newReaction.UserId });
        }

        public async Task<bool> Update(RatingReaction updatedReactions)
        {
            var updateReaction = @"UPDATE ratings_reactions 
                                   SET reaction = CAST(@Reaction AS reaction_type)
                                   WHERE id = @Id";
            return await connection.ExecuteAsync(updateReaction, new
            {
                Reaction = updatedReactions.Reaction.ToString().ToLower(),
                updatedReactions.Id
            }) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteReactionQuery = "DELETE FROM ratings_reactions WHERE id = @Id";
            return await connection.ExecuteAsync(deleteReactionQuery, new { Id = id }) > 0;
        }

        public async Task<IEnumerable<RatingReaction>> GetFiltered(string filter, string value)
        {
            var getFilteredMoviesQuery = $@"SELECT id AS Id, rating_id AS RatingId, reaction AS Reaction, user_id AS UserId
                                         FROM ratings_reactions
                                            WHERE {filter} = '{value}'";
            return await connection.QueryAsync<RatingReaction>(getFilteredMoviesQuery);
        }
    }
}
