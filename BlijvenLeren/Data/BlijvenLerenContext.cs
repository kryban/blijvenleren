using Microsoft.EntityFrameworkCore;
using BlijvenLeren.Models;

namespace BlijvenLeren.Data
{
    public class BlijvenLerenContext : DbContext
    {
        public BlijvenLerenContext (DbContextOptions<BlijvenLerenContext> options)
            : base(options)
        {
        }

        public DbSet<LearnResource> LearnResource { get; set; }
        public DbSet<Comment> Comment { get; set; }
    }
}
