using MediVora.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppointmentStatusConfiguration : IEntityTypeConfiguration<AppointmentStatus>
{
    public void Configure(EntityTypeBuilder<AppointmentStatus> builder)
    {
        builder.ToTable("AppointmentStatus");

        builder.HasKey(a => a.AppointmentStatusID);
        builder.Property(a => a.AppointmentStatusID).ValueGeneratedOnAdd();

        builder.Property(a => a.StatusCode).IsRequired().HasMaxLength(50);
        builder.Property(a => a.StatusName).IsRequired().HasMaxLength(100);
        builder.Property(a => a.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(a => a.CreatedBy).IsRequired();
        builder.Property(a => a.ModifiedBy).IsRequired();
    }
}