using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICT_151.Data
{
    public sealed class SQLiteApplicationDbContext : ApplicationDbContext
    {
        public SQLiteApplicationDbContext(DbContextOptions<SQLiteApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
        }
    }
}
