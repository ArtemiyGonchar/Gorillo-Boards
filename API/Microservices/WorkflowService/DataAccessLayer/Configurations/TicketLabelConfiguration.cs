using DataAccessLayer.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class TicketLabelConfiguration : IEntityTypeConfiguration<TicketLabel>
    {
        public void Configure(EntityTypeBuilder<TicketLabel> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(50)
                .IsRequired();
            //builder.HasIndex(x => x.Title).IsUnique();

            builder.HasIndex(x => new { x.BoardId, x.Title })
                .IsUnique();

            builder.HasOne(x => x.Board)
                .WithMany(b => b.Labels)
                .HasForeignKey(x => x.BoardId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
