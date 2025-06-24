using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class BoardConfiguration : IEntityTypeConfiguration<Board>
    {
        public void Configure(EntityTypeBuilder<Board> builder)
        {
            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();
            builder.HasIndex(x => x.Title).IsUnique();

            builder.Property(x => x.Description)
                .HasMaxLength(2048);
            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasMany(x => x.AllowedRoles)
                .WithOne(b => b.Board)
                .HasForeignKey(x => x.BoardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
