using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeTraveling.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool HideLocation { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
