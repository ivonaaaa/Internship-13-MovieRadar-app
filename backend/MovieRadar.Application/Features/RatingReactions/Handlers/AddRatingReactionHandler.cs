using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingReactions.Handlers
{
    public class AddRatingReactionHandler : IRequestHandler<AddRatingReactionCommand, int>
    {
        private readonly IRatingReactionRepository ratingReactionRepository;

        public AddRatingReactionHandler(IRatingReactionRepository ratingReactionRepository)
        {
            this.ratingReactionRepository = ratingReactionRepository;
        }

        public async Task<int> Handle(AddRatingReactionCommand request, CancellationToken cancellationToken)
        {
            var reactionValidation = ReactionHelper.IsReactionValid(request.reaction, ratingReactionRepository);
            if (!reactionValidation.Item1)
                throw new ArgumentException(reactionValidation.Item2);

            if (!await ReactionHelper.IsRatingReactionUnique(request.reaction, ratingReactionRepository))
                throw new ArgumentException("User already reacted to this rating.");

            try
            {
                return await ratingReactionRepository.Add(request.reaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while adding rating reaction: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}