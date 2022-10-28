using System.ComponentModel.DataAnnotations;

namespace BeTraveling.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required]
        public int Status { get; set; } = 1;
        [Required]
        public string Role { get; set; } = "Traveler";


    }
}
