using MediVora.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Data.EF.Configurations
{
    public class RolesConfiguration : IEntityTypeConfiguration<Roles>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.RoleID);
            builder.Property(x => x.RoleID).ValueGeneratedOnAdd();

            builder.Property(r => r.RoleCode).IsRequired().HasMaxLength(50);
            builder.Property(r => r.RoleName).IsRequired().HasMaxLength(100);
            builder.Property(r => r.RoleDescription).HasMaxLength(250);
            builder.Property(r => r.IsActive).IsRequired();
            builder.Property(r => r.CreatedBy).IsRequired();
            builder.Property(r => r.ModifiedBy).IsRequired();

            builder.HasMany(r => r.Users)
                   .WithOne(u => u.Roles)
                   .HasForeignKey(u => u.RoleID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
