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
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasMany(x => x.Tickets)
                .WithOne(t => t.State)
                .HasForeignKey(t => t.StateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Board)
                .WithMany(b => b.States)
                .HasForeignKey(x => x.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
