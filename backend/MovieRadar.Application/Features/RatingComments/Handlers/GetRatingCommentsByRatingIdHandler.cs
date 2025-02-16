using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingComments.Handlers
{
    public class GetRatingCommentsByRatingIdHandler : IRequestHandler<GetRatingCommentsByRatingIdQuery, IEnumerable<RatingComment>>
    {
        private readonly IRatingCommentRepository ratingCommentRepository;

        public GetRatingCommentsByRatingIdHandler(IRatingCommentRepository ratingCommentRepository)
        {
            this.ratingCommentRepository = ratingCommentRepository;
        }

        public async Task<IEnumerable<RatingComment>> Handle(GetRatingCommentsByRatingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingCommentRepository.GetAllByRatingId(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all reactions by rating id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}