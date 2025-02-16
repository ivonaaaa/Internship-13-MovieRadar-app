using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.RatingComments.Handlers
{
    public class GetAllRatingCommentsHandler : IRequestHandler<GetAllRatingCommentsQuery, IEnumerable<RatingComment>>
    {
        private readonly IRatingCommentRepository ratingCommentRepository;

        public GetAllRatingCommentsHandler(IRatingCommentRepository ratingCommentRepository)
        {
            this.ratingCommentRepository = ratingCommentRepository;
        }

        public async Task<IEnumerable<RatingComment>> Handle(GetAllRatingCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingCommentRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all movies: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}