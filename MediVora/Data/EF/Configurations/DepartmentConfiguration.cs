using MediVora.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Data.EF.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(d => d.DepartmentId);
            builder.Property(d => d.DepartmentId).ValueGeneratedOnAdd();

            builder.Property(d => d.DepartmentCode).IsRequired().HasColumnType("varchar(50) ");
            builder.Property(d => d.DepartmentName).IsRequired().HasMaxLength(100);
            builder.Property(d => d.Description).HasMaxLength(500).IsRequired(false);
            builder.Property(d => d.IsActive).IsRequired();
            builder.Property(d => d.CreatedBy).IsRequired();
            builder.Property(d => d.ModifiedBy).IsRequired();

            builder.HasMany(d => d.Doctors)
                .WithOne(d => d.Department)
                .HasForeignKey(d => d.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
