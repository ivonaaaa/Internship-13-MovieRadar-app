using MediatR;
using MovieRadar.Domain.Entities;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Ratings.Handlers
{
    public class GetRatingByIdHandler : IRequestHandler<GetRatingByIdQuery, Rating?>
    {
        private readonly IRatingRepository ratingRepository;

        public GetRatingByIdHandler(IRatingRepository ratingRepository)
        {
            this.ratingRepository = ratingRepository;
        }

        public async Task<Rating?> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await ratingRepository.GetById(request.id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting rating by id: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }
}