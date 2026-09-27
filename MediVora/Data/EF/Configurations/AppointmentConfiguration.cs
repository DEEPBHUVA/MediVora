using MediVora.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointment");

        builder.HasKey(a => a.AppointmentID);
        builder.Property(a => a.AppointmentID).ValueGeneratedOnAdd();

        builder.Property(a => a.AppointmentNo).IsRequired().HasMaxLength(40);
        builder.HasIndex(a => a.AppointmentNo).IsUnique();

        builder.Property(a => a.DoctorScheduleID).IsRequired();
        builder.Property(a => a.DoctorID).IsRequired();
        builder.Property(a => a.PatientID).IsRequired();
        builder.Property(a => a.AppointmentStatusID).IsRequired();
        builder.Property(a => a.AppointmentDate).IsRequired().HasColumnType("date");
        builder.Property(a => a.StartTime).IsRequired().HasColumnType("time(0)");
        builder.Property(a => a.EndTime).IsRequired().HasColumnType("time(0)");
        builder.Property(a => a.AppointmentType).HasMaxLength(30);
        builder.Property(a => a.Reason).HasMaxLength(1000);
        builder.Property(a => a.PatientRemarks).IsRequired(false).HasMaxLength(1000);
        builder.Property(a => a.DoctorRemarks).IsRequired(false).HasMaxLength(1000);
        builder.Property(a => a.BookedDate).IsRequired().HasColumnType("datetime");
        builder.Property(a => a.ConfirmedDate).IsRequired(false).HasColumnType("datetime");
        builder.Property(a => a.CheckedInDate).IsRequired(false).HasColumnType("datetime");
        builder.Property(a => a.StartedDate).IsRequired(false).HasColumnType("datetime");
        builder.Property(a => a.CompletedDate).IsRequired(false).HasColumnType("datetime");
        builder.Property(a => a.CancelledDate).IsRequired(false).HasColumnType("datetime");
        builder.Property(a => a.CancellationReason).IsRequired(false).HasMaxLength(500);
        builder.Property(a => a.CreatedBy);
        builder.Property(a => a.ModifiedBy);

        builder.HasOne(a => a.DoctorSchedule)
                .WithMany(ds => ds.Appointments)
                .HasForeignKey(a => a.DoctorScheduleID)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
               .WithMany(d => d.Appointments)
               .HasForeignKey(a => a.DoctorID)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Patient)
               .WithMany(p => p.Appointments)
               .HasForeignKey(a => a.PatientID)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.AppointmentStatus)
               .WithMany(s => s.Appointments)
               .HasForeignKey(a => a.AppointmentStatusID)
               .OnDelete(DeleteBehavior.Restrict);
    }
}