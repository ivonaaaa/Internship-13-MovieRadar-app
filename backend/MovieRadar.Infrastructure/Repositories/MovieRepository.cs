using MovieRadar.Domain.Interfaces;
using System.Data;
using MovieRadar.Domain.Entities;
using Dapper;

namespace MovieRadar.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IDbConnection connection;
        public MovieRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            var getAllQuery = @"SELECT id AS Id, title AS Title, summary AS Summary, 
                                       release_year AS ReleaseYear, genre AS Genre
                                FROM movies ";
            return await connection.QueryAsync<Movie>(getAllQuery);
        }

        public async Task<Movie?> GetById(int id)
        {
            var getByIdQuery = @"SELECT id AS Id, title AS Title, summary AS Summary, 
                                       release_year AS ReleaseYear, genre AS Genre
                                 FROM movies
                                 WHERE id = @Id";
            return await connection.QuerySingleOrDefaultAsync<Movie>(getByIdQuery, new { Id = id });
        }

        public async Task<int> Add(Movie newMovie)
        {
            var addMovieQuery = @"INSERT INTO movies(title, summary, release_year, genre) 
                                    VALUES (@Title, @Summary, @ReleaseYear, @Genre)
                                    RETURNING id AS Id";
            return await connection.ExecuteScalarAsync<int>(addMovieQuery, newMovie);
        }

        public async Task<bool> Update(Movie newMovie)
        {
            var updateMovieQuery = @"UPDATE movies SET title = @Title, summary = @Summary, release_year = @ReleaseYear, genre = @Genre 
                                     WHERE id = @Id";
            return await connection.ExecuteAsync(updateMovieQuery, newMovie) > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var deleteMovieQuery = "DELETE FROM movies WHERE id = @Id";
            return await connection.ExecuteAsync(deleteMovieQuery, new { Id = id }) > 0;
        }
    }
}
