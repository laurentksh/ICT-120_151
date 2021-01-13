using System;
using System.Collections.Generic;
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


        public DbSet<Submission> Submissions { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<Retweet> Retweets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
