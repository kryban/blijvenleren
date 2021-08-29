using Microsoft.EntityFrameworkCore;
using BlijvenLeren.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BlijvenLeren.Data
{
    public class BlijvenLerenContext : IdentityDbContext
    {
        public BlijvenLerenContext (DbContextOptions<BlijvenLerenContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<LearnResource> LearnResource { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
