using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Movies.Handlers
{
    public class GetMovieByIdHandler : IRequestHandler<GetMovieByIdQuery, Movie?>
    {
        private readonly IMovieRepository movieRepository;

        public GetMovieByIdHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<Movie?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await movieRepository.GetById(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting movie by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}