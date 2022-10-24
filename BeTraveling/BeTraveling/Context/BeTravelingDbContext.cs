using BeTraveling.Models;
using Microsoft.EntityFrameworkCore;

namespace BeTraveling.Context
{
    public class BeTravelingDbContext : DbContext
    {

        public BeTravelingDbContext(DbContextOptions<BeTravelingDbContext> options) : base(options)    
        {
                
        }
        public DbSet<User> Users { get; set; }
        
    }
}
