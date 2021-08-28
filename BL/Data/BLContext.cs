using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BL.Models;

namespace BL.Data
{
    public class BLContext : DbContext
    {
        public BLContext (DbContextOptions<BLContext> options)
            : base(options)
        {
        }

        public DbSet<BL.Models.LearnResourceModel> LearnResourceModel { get; set; }

        public DbSet<BL.Models.CommentModel> CommentModel { get; set; }
    }
}
