using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace RatingRadar.Application.Services.Ratings.Handlers
{
    public class GetAllRatingsHandler : IRequestHandler<GetAllRatingsQuery, IEnumerable<Rating>>
    {
        private readonly IRatingRepository ratingRepository;

        public GetAllRatingsHandler(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public async Task<IEnumerable<Rating>> Handle(GetAllRatingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while getting all ratings: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}