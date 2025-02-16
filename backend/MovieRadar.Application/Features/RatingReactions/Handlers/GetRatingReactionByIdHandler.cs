using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingReactions.Handlers
{
    public class GetRatingReactionByIdHandler : IRequestHandler<GetRatingReactionByIdQuery, RatingReaction?>
    {
        private readonly IRatingReactionRepository ratingReactionRepository;

        public GetRatingReactionByIdHandler(IRatingReactionRepository ratingReactionRepository)
        {
            this.ratingReactionRepository = ratingReactionRepository;
        }

        public async Task<RatingReaction?> Handle(GetRatingReactionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingReactionRepository.GetById(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting reaction by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}