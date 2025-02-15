using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Movies.Handlers
{
    public class GetFilteredMoviesHandler : IRequestHandler<GetFilteredMoviesQuery, IEnumerable<Movie>>
    {
        private readonly IMovieRepository movieRepository;

        public GetFilteredMoviesHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> Handle(GetFilteredMoviesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await movieRepository.GetFilteredMovies(request.filter, request.parameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting filtered movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}