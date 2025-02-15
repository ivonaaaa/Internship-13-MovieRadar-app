using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services.Movies.Handlers
{
    public class GetOrderedMoviesByGradeHandler : IRequestHandler<GetOrderedMoviesByGradeQuery, IEnumerable<Movie>>
    {
        private readonly IMovieRepository movieRepository;

        public GetOrderedMoviesByGradeHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> Handle(GetOrderedMoviesByGradeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await movieRepository.OrderByRating(request.orderDirection);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting ordered movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}