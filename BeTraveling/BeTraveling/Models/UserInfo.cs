namespace BeTraveling.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public List<User> Friends { get; set; }
        public List<Post> Posts { get; set; }
        public List<Reaction> Reactions { get; set; }
        public bool HideLocation { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
