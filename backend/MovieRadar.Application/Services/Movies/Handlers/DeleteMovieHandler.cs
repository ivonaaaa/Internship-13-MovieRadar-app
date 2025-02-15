using MediatR;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Services.Movies.Handlers
{
    public class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, bool>
    {
        private readonly IMovieRepository movieRepository;

        public DeleteMovieHandler(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await movieRepository.Delete(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}