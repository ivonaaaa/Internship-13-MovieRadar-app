using MediatR;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Ratings.Handlers
{
    public class DeleteRatingHandler : IRequestHandler<DeleteRatingCommand, bool>
    {
        private readonly IRatingRepository ratingRepository;

        public DeleteRatingHandler(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public async Task<bool> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingRepository.Delete(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting rating: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}