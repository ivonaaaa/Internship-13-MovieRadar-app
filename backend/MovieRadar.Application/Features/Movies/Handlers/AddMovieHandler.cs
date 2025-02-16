using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Movies.Handlers
{
    public class AddMovieHandler : IRequestHandler<AddMovieCommand, int>
    {
        private readonly IMovieRepository movieRepository;

        public AddMovieHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<int> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            var newMovieValidation = MovieHelper.IsMovieValid(request.movie);
            if (!newMovieValidation.Item1)
                throw new ArgumentException(newMovieValidation.Item2);

            try
            {
                return await movieRepository.Add(request.movie);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}