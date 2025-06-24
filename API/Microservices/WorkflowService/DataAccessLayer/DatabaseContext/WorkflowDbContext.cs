using DataAccessLayer.Configurations;
using DataAccessLayer.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DatabaseContext
{
    public class WorkflowDbContext : DbContext
    {
        public WorkflowDbContext(DbContextOptions<WorkflowDbContext> options) : base(options) { }

        public DbSet<State> States { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketBoard> TicketBoards { get; set; }
        public DbSet<TicketLabel> Labels { get; set; }
        public DbSet<TicketTimeLog> TimeLog { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StateConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new TicketLabelConfiguration());
            modelBuilder.ApplyConfiguration(new TicketTimeLogConfiguration());
            modelBuilder.ApplyConfiguration(new TicketBoardConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
