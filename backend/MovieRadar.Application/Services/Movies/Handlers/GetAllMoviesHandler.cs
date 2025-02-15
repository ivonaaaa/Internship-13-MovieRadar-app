using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services.Movies.Handlers
{
    public class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, IEnumerable<Movie>>
    {
        private readonly IMovieRepository movieRepository;

        public GetAllMoviesHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await movieRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}