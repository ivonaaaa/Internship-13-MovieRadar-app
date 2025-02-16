namespace MovieRadar.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public string? Review { get; set; }
        public float Grade { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; }
    }
}
