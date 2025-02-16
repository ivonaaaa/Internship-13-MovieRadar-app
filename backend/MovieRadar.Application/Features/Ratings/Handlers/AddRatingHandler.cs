using MediatR;
using MovieRadar.Application.Helpers;
using MovieRadar.Domain.Interfaces;

namespace MovieRadar.Application.Features.Ratings.Handlers
{
    public class AddRatingHandler : IRequestHandler<AddRatingCommand, int>
    {
        private readonly IRatingRepository ratingRepository;
        private readonly IMovieRepository movieRepository;

        public AddRatingHandler(IRatingRepository ratingRepository, IMovieRepository movieRepository)
        {
            this.ratingRepository = ratingRepository;
            this.movieRepository = movieRepository;
        }

        public async Task<int> Handle(AddRatingCommand request, CancellationToken cancellationToken)
        {
            var updateRatingValidation = await RatingHelper.IsRatingValid(request.rating, movieRepository);
            if (!updateRatingValidation.Item1)
                throw new ArgumentException(updateRatingValidation.Item2);

            try
            {
                return await ratingRepository.Add(request.rating);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new rating: {ex.Message}, inner: {ex.InnerException}");
            }
        }
    }

}