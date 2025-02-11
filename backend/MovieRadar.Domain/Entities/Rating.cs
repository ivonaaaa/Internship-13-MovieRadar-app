

namespace MovieRadar.Domain.Entities
{
    public class Rating
    {
        public int RatingId { get; set; }
        public string Review { get; set; }
        public int Grade { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
