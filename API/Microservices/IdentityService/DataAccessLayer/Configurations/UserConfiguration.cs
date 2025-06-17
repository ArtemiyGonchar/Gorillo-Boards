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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) //configuring user for ef
        {
            builder.Property(x => x.UserName).HasMaxLength(100).IsRequired();
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.DisplayName).HasMaxLength(100).IsRequired();

            builder.Property(x => x.Role).HasConversion<string>().IsRequired().HasMaxLength(20); //in db it will be displayed as "admin" or "member", not 1 or 2
            builder.Property(x => x.AvatarUrl).HasMaxLength(2048);
            builder.Property(x => x.PasswordHash).HasMaxLength(256).IsRequired();
            builder.Property(x => x.Salt).HasMaxLength(256).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
        }
    }
}
