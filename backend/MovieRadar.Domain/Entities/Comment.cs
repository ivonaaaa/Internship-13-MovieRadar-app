

namespace MovieRadar.Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Review { get; set; }
        public int Rating { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
