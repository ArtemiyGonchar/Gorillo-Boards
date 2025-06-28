using DataAccessLayer.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class TicketTimeLogConfiguration : IEntityTypeConfiguration<TicketTimeLog>
    {
        public void Configure(EntityTypeBuilder<TicketTimeLog> builder)
        {
            builder.HasOne(x => x.Ticket)
                .WithMany(t => t.TimeLogs)
                .HasForeignKey(t => t.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.StartedAt).IsRequired();
            //builder.Property(x => x.EndedAt).IsRequired();
        }
    }
}
