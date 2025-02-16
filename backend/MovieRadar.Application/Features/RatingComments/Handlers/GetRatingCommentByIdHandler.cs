using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingComments.Handlers
{
    public class GetRatingCommentByIdHandler : IRequestHandler<GetRatingCommentByIdQuery, RatingComment?>
    {
        private readonly IRatingCommentRepository ratingCommentRepository;

        public GetRatingCommentByIdHandler(IRatingCommentRepository ratingCommentRepository)
        {
            this.ratingCommentRepository = ratingCommentRepository;
        }

        public async Task<RatingComment?> Handle(GetRatingCommentByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingCommentRepository.GetById(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting movie by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}