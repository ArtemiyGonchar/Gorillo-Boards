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
    public class TicketBoardConfiguration : IEntityTypeConfiguration<TicketBoard>
    {
        public void Configure(EntityTypeBuilder<TicketBoard> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.Title).IsUnique();

            builder.HasMany(x => x.States)
                .WithOne(s => s.Board)
                .HasForeignKey(s => s.BoardId);
        }
    }
}
