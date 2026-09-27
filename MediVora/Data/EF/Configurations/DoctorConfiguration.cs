using MediVora.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediVora.Data.EF.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctors>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Doctors> builder)
        {
            builder.ToTable("Doctors");
            builder.HasKey(d => d.DoctorID);
            builder.Property(d => d.DoctorID).ValueGeneratedOnAdd();

            builder.Property(d => d.UserID).IsRequired();
            builder.Property(d => d.DepartmentID).IsRequired();
            builder.Property(d => d.DoctorCode).HasMaxLength(50).IsRequired();
            builder.Property(d => d.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(d => d.MiddleName).HasMaxLength(100).IsRequired();
            builder.Property(d => d.LastName).HasMaxLength(100).IsRequired();
            builder.Property(d => d.Qualification).HasMaxLength(200).IsRequired();
            builder.Property(d => d.Specialization).HasMaxLength(200).IsRequired();
            builder.Property(d => d.MedicalRegistrationNo).HasMaxLength(100).IsRequired();
            builder.Property(d => d.ConsultationFee).HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(d => d.ProfileImagePath).HasMaxLength(200).IsRequired();
            builder.Property(d => d.IsActive).IsRequired();
            builder.Property(d => d.IsTrashed).IsRequired().HasDefaultValue(false);
            builder.Property(d => d.CreatedBy).IsRequired();
            builder.Property(d => d.ModifiedBy).IsRequired();

            builder.HasOne(d => d.Department)
                .WithMany(d => d.Doctors)
                .HasForeignKey(d => d.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.User)
                .WithMany(u => u.Doctors)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.DoctorScheduleTemplates)
                .WithOne(s => s.Doctor)
                .HasForeignKey(s => s.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.DoctorSchedules)
                .WithOne(s => s.Doctor)
                .HasForeignKey(s => s.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
