using MediVora.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediVora.Data.EF.Configurations
{
    public class PatientsConfiguration : IEntityTypeConfiguration<Patients>
    {
        public void Configure(EntityTypeBuilder<Patients> builder)
        {
            builder.ToTable("Patients");
            builder.HasKey(p => p.PatientID);
            builder.Property(p => p.PatientID).ValueGeneratedOnAdd();

            builder.Property(p => p.UserID).IsRequired();
            builder.Property(p => p.PatientCode).IsRequired().HasMaxLength(30);
            builder.HasIndex(p => p.PatientCode).IsUnique();
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.MiddleName).IsRequired(false).HasMaxLength(100);
            builder.Property(p => p.LastName).IsRequired(false).HasMaxLength(100);
            builder.Property(p => p.DateOfBirth).IsRequired(false).HasColumnType("date");
            builder.Property(p => p.Gender).IsRequired(false).HasMaxLength(20);
            builder.Property(p => p.BloodGroup).IsRequired(false).HasMaxLength(10);
            builder.Property(p => p.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(p => p.City).IsRequired(false).HasMaxLength(100);
            builder.Property(p => p.State).IsRequired(false).HasMaxLength(100);
            builder.Property(p => p.PostalCode).IsRequired(false).HasMaxLength(20);
            builder.Property(p => p.EmergencyContactName).IsRequired(false).HasMaxLength(150);
            builder.Property(p => p.EmergencyContactNumber).IsRequired(false).HasMaxLength(20);
            builder.Property(p => p.IsActive).IsRequired();

            builder.HasOne(d => d.User)
                .WithMany(u => u.Patients)
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.UserID).IsUnique();
        }
    }
}
