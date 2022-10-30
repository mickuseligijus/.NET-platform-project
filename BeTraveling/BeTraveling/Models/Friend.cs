using System.ComponentModel.DataAnnotations.Schema;

namespace BeTraveling.Models
{
    public class Friend
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId1 { get; set; } //the one who invited
        [ForeignKey("User")]
        public int UserId2 { get; set; } //who was invited
        public int Status { get; set; } //0-friends, 1-request sent, pending for response
    }
}
