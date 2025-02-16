using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingComments.Handlers
{
    public class GetFilteredRatingCommentsHandler : IRequestHandler<GetFilteredRatingCommentsQuery, IEnumerable<RatingComment>>
    {
        private readonly IRatingCommentRepository ratingCommentRepository;

        public GetFilteredRatingCommentsHandler(IRatingCommentRepository ratingCommentRepository)
        {
            this.ratingCommentRepository = ratingCommentRepository;
        }

        public async Task<IEnumerable<RatingComment>> Handle(GetFilteredRatingCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingCommentRepository.GetFiltered(request.filter, request.parameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting filtered comments: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}