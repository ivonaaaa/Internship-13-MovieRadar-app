﻿

namespace MovieRadar.Domain.Entities
{
    public class RatingsComments
    {
        public int Id { get; set; }
        public int RatingId { get; set; }
        public int MovieId { get; set; }
        public string Comment { get; set; }
    }
}
