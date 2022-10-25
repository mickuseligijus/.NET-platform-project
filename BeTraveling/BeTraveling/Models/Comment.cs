﻿namespace BeTraveling.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string  Text { get; set; }

        public List<Reaction> Reactions { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
