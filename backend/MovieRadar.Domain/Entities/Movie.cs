namespace MovieRadar.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear{ get; set; }
        public string Summary { get; set; }
        public float AvgRating { get; }
        public DateTime CreatedAt { get; }
        public DateTime LastModifiedAt { get; }
        public string ImageLink { get; set; }
    }
}
