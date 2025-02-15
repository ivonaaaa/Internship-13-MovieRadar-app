using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Ratings.Handlers
{
    public class UpdateRatingHandler : IRequestHandler<UpdateRatingCommand, bool>
    {
        private readonly IRatingRepository ratingRepository;
        private readonly IMovieRepository movieRepository;

        public UpdateRatingHandler(IRatingRepository ratingRepository, IMovieRepository movieRepository)
        {
            this.ratingRepository = ratingRepository;
            this.movieRepository = movieRepository;
        }

        public async Task<bool> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
        {
            var updateRatingValidation = await RatingHelper.IsRatingValid(request.rating, movieRepository);
            if (!updateRatingValidation.Item1)
                throw new ArgumentException(updateRatingValidation.Item2);

            try
            {
                return await ratingRepository.Update(request.rating);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating rating: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}