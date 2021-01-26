using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Models;
using Microsoft.EntityFrameworkCore;

namespace ICT_151.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }


        public DbSet<Publication> Publications { get; set; }
        
        public DbSet<Like> Likes { get; set; }
        
        /// <summary>A.k.a Retweets</summary>
        public DbSet<Repost> Reposts { get; set; }

        public DbSet<Follow> Follows { get; set; }

        public DbSet<Block> Blocks { get; set; }

        public DbSet<PrivateMessage> PrivateMessages { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }
}