namespace BeTraveling.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
    }
}
