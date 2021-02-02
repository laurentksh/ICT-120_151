/*using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Data
{
    public sealed class SQLServerApplicationDbContext : ApplicationDbContext
    {
        public SQLServerApplicationDbContext(DbContextOptions<SQLServerApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<Block>()
                .HasOne(x => x.Blocker)
                .WithMany(x => x.Blocking)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Block>()
                .HasOne(x => x.BlockTarget)
                .WithMany(x => x.Blocked)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Follow>()
                .HasOne(x => x.Follower)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Follow>()
                .HasOne(x => x.FollowTarget)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);*\/
        }
    }
}
*/