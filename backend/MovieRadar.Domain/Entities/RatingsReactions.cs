﻿

namespace MovieRadar.Domain.Entities
{
    public class RatingsReactions
    {
        public int Id { get; set; }
        public int RatingId { get; set; }
        public int UserId { get; set; }
        public string Reaction { get; set; }
    }
}
