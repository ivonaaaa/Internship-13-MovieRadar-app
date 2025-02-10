

namespace MovieRadar.Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int Grade { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
