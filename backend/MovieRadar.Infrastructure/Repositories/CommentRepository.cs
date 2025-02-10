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
    public class CommentRepository : ICommentRepository
    {
        private readonly IDbConnection connection;
        public CommentRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            var getAllQuery = "SELECT * FROM comments";
            return await connection.QueryAsync<Comment>(getAllQuery);
        }

        public async Task<Comment?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS CommentId, user_id AS UserId, movie_id AS MovieId,review AS Review, 
                                   rating AS Rating, created_at AS CreatedAt 
                            FROM comments 
                            WHERE id = @CommentId";
            return await connection.QuerySingleOrDefaultAsync<Comment>(getByIdQuery, new { CommentId = id });
        }

        public async Task<int> Add(Comment newComment)
        {
            var addCommentQuery = @"INSERT INTO comments(user_id, movie_id, rating, review) 
                                    VALUES (@UserId, @MovieId, @Rating, @Review);
                                    SELECT LAST_INSERTED_ID";
            return await connection.ExecuteScalarAsync<int>(addCommentQuery, newComment);
        }

        public async Task<bool> Update(Comment newComment)
        {
            var updateCommentQuery = "UPDATE comments SET rating = @Rating, review = @Review";

            return await connection.ExecuteAsync(updateCommentQuery, newComment) > 0;
        }
        public async Task<bool> Delete(int id)
        {
            Console.WriteLine(id);
            var deleteCommentQuery = "DELETE FROM comments WHERE id = @CommentId";

            return await connection.ExecuteAsync(deleteCommentQuery, new { CommentId = id }) > 0;
        }
    }
}
