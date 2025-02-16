using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingReactions.Handlers
{
    public class GetAllRatingReactionsHandler : IRequestHandler<GetAllRatingReactionsQuery, IEnumerable<RatingReaction>>
    {
        private readonly IRatingReactionRepository ratingReactionRepository;

        public GetAllRatingReactionsHandler(IRatingReactionRepository ratingReactionRepository)
        {
            this.ratingReactionRepository = ratingReactionRepository;
        }

        public async Task<IEnumerable<RatingReaction>> Handle(GetAllRatingReactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingReactionRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all reactions: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}