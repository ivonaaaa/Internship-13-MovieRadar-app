namespace MovieRadar.Domain.Entities
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ReleaseDate { get; set; }
        public string Rating { get; set; }
        //public string Image { get; set; }
    }
}
