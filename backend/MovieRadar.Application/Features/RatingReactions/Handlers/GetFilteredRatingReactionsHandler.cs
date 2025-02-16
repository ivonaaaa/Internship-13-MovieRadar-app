using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingReactions.Handlers
{
    public class GetRatingReactionsByRatingIdHandler : IRequestHandler<GetFilteredRatingReactionsQuery, IEnumerable<RatingReaction>>
    {
        private readonly IRatingReactionRepository ratingReactionRepository;

        public GetRatingReactionsByRatingIdHandler(IRatingReactionRepository ratingReactionRepository)
        {
            this.ratingReactionRepository = ratingReactionRepository;
        }

        public async Task<IEnumerable<RatingReaction>> Handle(GetFilteredRatingReactionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingReactionRepository.GetFiltered(request.filter, request.value);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting filtered reactions: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}