using MediatR;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingComments.Handlers
{
    public class DeleteRatingCommentHandler : IRequestHandler<DeleteRatingCommentCommand, bool>
    {
        private readonly IRatingCommentRepository ratingCommentRepository;

        public DeleteRatingCommentHandler(IRatingCommentRepository ratingCommentRepository)
        {
            this.ratingCommentRepository = ratingCommentRepository;
        }

        public async Task<bool> Handle(DeleteRatingCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingCommentRepository.Delete(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}