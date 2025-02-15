using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Movies.Handlers
{
    public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, bool>
    {
        private readonly IMovieRepository movieRepository;

        public UpdateMovieHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<bool> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var updateMovieValidation = MovieHelper.IsMovieValid(request.movie);
            if (!updateMovieValidation.Item1)
                throw new ArgumentException(updateMovieValidation.Item2);

            try
            {
                return await movieRepository.Update(request.movie);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}