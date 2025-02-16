using MediatR;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingReactions.Handlers
{
    public class DeleteRatingReactionHandler : IRequestHandler<DeleteRatingReactionCommand, bool>
    {
        private readonly IRatingReactionRepository ratingReactionRepository;

        public DeleteRatingReactionHandler(IRatingReactionRepository ratingReactionRepository)
        {
            this.ratingReactionRepository = ratingReactionRepository;
        }

        public async Task<bool> Handle(DeleteRatingReactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingReactionRepository.Delete(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting reaction: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}