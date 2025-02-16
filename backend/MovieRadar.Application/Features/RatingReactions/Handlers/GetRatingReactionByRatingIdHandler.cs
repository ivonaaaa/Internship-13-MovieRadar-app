using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingReactions.Handlers
{
    public class GetRatingReactionsByRatingIdHandler : IRequestHandler<GetRatingReactionsByRatingIdQuery, IEnumerable<RatingReaction>>
    {
        private readonly IRatingReactionRepository ratingReactionRepository;

        public GetRatingReactionsByRatingIdHandler(IRatingReactionRepository ratingReactionRepository)
        {
            this.ratingReactionRepository = ratingReactionRepository;
        }

        public async Task<IEnumerable<RatingReaction>> Handle(GetRatingReactionsByRatingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingReactionRepository.GetAllByRatingId(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all reactions by rating id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}