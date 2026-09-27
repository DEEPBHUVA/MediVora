using MediVora.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Data.EF.Configurations
{
    public class DoctorScheduleConfiguration : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.ToTable("DoctorSchedules");
            builder.HasKey(ds => ds.DoctorScheduleID);
            builder.Property(ds => ds.DoctorScheduleID).ValueGeneratedOnAdd();

            builder.Property(ds => ds.DoctorID).IsRequired();
            builder.Property(ds => ds.ScheduleDate).IsRequired();
            builder.Property(ds => ds.StartTime).IsRequired();
            builder.Property(ds => ds.EndTime).IsRequired();
            builder.Property(ds => ds.MaxAppointments).IsRequired();
            builder.Property(ds => ds.IsAvailable).IsRequired();
            builder.Property(ds => ds.Remarks).HasMaxLength(500).IsRequired(false);
            builder.Property(ds => ds.CreatedBy).IsRequired();
            builder.Property(ds => ds.ModifiedBy).IsRequired();

            builder.HasOne(ds => ds.Doctor)
                .WithMany(d => d.DoctorSchedules)
                .HasForeignKey(ds => ds.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
