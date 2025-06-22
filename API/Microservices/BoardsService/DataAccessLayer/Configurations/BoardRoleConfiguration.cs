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
    public class BoardRoleConfiguration : IEntityTypeConfiguration<BoardRole>
    {
        public void Configure(EntityTypeBuilder<BoardRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AllowedRoles).HasConversion<string>().IsRequired();

            builder.HasOne(x => x.Board).WithMany(b => b.AllowedRoles).HasForeignKey(x => x.BoardId);
        }
    }
}
