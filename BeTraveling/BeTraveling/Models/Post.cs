namespace BeTraveling.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}