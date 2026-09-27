using MediVora.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Data.EF.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.UserID);
            builder.Property(x => x.UserID).ValueGeneratedOnAdd();

            builder.Property(u => u.RoleID).IsRequired();
            builder.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(100);
            builder.Property(u => u.MobileNo).HasMaxLength(15);
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.LastLoginDate).IsRequired(false);

            builder.HasOne(u => u.Roles)
                   .WithMany(r => r.Users)
                   .HasForeignKey(u => u.RoleID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Doctors)
                   .WithOne(d => d.User)
                   .HasForeignKey(d => d.UserID)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
