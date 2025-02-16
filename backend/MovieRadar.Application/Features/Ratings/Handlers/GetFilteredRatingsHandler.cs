using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Ratings.Handlers
{
    public class GetFilteredRatingsHandler : IRequestHandler<GetFilteredRatingsQuery, IEnumerable<Rating>>
    {
        private readonly IRatingRepository ratingRepository;

        public GetFilteredRatingsHandler(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public async Task<IEnumerable<Rating>> Handle(GetFilteredRatingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingRepository.GetFiltered(request.filter, request.parameter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting filtered ratings: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}