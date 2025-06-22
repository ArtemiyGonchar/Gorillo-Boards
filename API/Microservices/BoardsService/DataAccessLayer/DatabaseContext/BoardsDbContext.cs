using DataAccessLayer.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DatabaseContext
{
    public class BoardsDbContext : DbContext
    {
        public BoardsDbContext(DbContextOptions<BoardsDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BoardConfiguration());
            modelBuilder.ApplyConfiguration(new BoardRoleConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
