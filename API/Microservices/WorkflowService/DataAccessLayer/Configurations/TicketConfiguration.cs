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
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Description).HasMaxLength(2048);

            builder.Property(x => x.IsClosed).IsRequired();

            builder.HasOne(x => x.State)
                .WithMany(s => s.Tickets)
                .HasForeignKey(x => x.StateId);

            builder.HasOne(x => x.TicketLabel)
                .WithMany()
                .HasForeignKey(x => x.TicketLabelId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.CreatedAt).IsRequired();

            builder.HasMany(x => x.TimeLogs)
                .WithOne(l => l.Ticket)
                .HasForeignKey(l => l.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
