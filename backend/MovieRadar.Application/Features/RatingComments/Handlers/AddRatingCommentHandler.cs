using MediatR;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingComments.Handlers
{
    public class AddRatingCommentHandler : IRequestHandler<AddRatingCommentCommand, int>
    {
        private readonly IRatingCommentRepository ratingCommentsRepository;

        public AddRatingCommentHandler(IRatingCommentRepository ratingCommentsRepository)
        {
            this.ratingCommentsRepository = ratingCommentsRepository;
        }

        public async Task<int> Handle(AddRatingCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingCommentsRepository.Add(request.comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}