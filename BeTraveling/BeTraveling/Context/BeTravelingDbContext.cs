using BeTraveling.Models;
using Microsoft.EntityFrameworkCore;

namespace BeTraveling.Context
{
    public class BeTravelingDbContext : DbContext
    {

        public BeTravelingDbContext(DbContextOptions<BeTravelingDbContext> options) : base(options)    
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserInfo>(entity => {
                entity.HasIndex(e => e.UserId).IsUnique();
            });

            builder.Entity<Friend>(entity => {
                entity.HasIndex(e => new { e.UserId1,e.UserId2 }).IsUnique();
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UsersInfo { get; set; }
        public DbSet<Friend> Friends { get; set; }
        
    }
}
