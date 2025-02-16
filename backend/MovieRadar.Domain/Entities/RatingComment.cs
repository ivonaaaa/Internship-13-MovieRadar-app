

namespace MovieRadar.Domain.Entities
{
    public class RatingComment
    {
        public int Id { get; set;  }
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
    }
}
