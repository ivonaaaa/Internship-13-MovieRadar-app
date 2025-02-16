using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingReactions.Handlers
{
    public class UpdateRatingReactionHandler : IRequestHandler<UpdateRatingReactionCommand, bool>
    {
        private readonly IRatingReactionRepository ratingReactionRepository;

        public UpdateRatingReactionHandler(IRatingReactionRepository ratingReactionRepository)
        {
            this.ratingReactionRepository = ratingReactionRepository;
        }

        public async Task<bool> Handle(UpdateRatingReactionCommand request, CancellationToken cancellationToken)
        {
            var reactionValidation = ReactionHelper.IsReactionValid(request.reaction, ratingReactionRepository);
            if (!reactionValidation.Item1)
                throw new ArgumentException(reactionValidation.Item2);

            try
            {
                return await ratingReactionRepository.Update(request.reaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating reaction: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}