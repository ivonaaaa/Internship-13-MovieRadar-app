using MediatR;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingComments.Handlers
{
    public class UpdateRatingCommentHandler : IRequestHandler<UpdateRatingCommentCommand, bool>
    {
        private readonly IRatingCommentRepository ratingCommentRepository;

        public UpdateRatingCommentHandler(IRatingCommentRepository ratingCommentRepository)
        {
            this.ratingCommentRepository = ratingCommentRepository;
        }

        public async Task<bool> Handle(UpdateRatingCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingCommentRepository.Update(request.comment);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating movie: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}