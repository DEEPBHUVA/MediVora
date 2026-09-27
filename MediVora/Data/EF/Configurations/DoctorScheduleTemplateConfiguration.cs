using MediVora.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediVora.Data.EF.Configurations
{
    public class DoctorScheduleTemplateConfiguration : IEntityTypeConfiguration<DoctorScheduleTemplate>
    {
        public void Configure(EntityTypeBuilder<DoctorScheduleTemplate> builder)
        {
            builder.ToTable("DoctorScheduleTemplates");
            builder.HasKey(d => d.DoctorScheduleTemplateID);
            builder.Property(d => d.DoctorScheduleTemplateID).ValueGeneratedOnAdd();

            builder.Property(d => d.DoctorID).IsRequired();
            builder.Property(d => d.DayOfWeek).IsRequired();
            builder.Property(d => d.DayOfWeekName).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(d => d.StartTime).IsRequired();
            builder.Property(d => d.EndTime).IsRequired();
            builder.Property(d => d.SlotDurationInMinutes).IsRequired();
            builder.Property(d => d.MaxAppointmentsPerSlot).IsRequired();
            builder.Property(d => d.EffectiveFromDate).IsRequired();
            builder.Property(d => d.EffectiveToDate).IsRequired(false);
            builder.Property(d => d.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(d => d.CreatedBy).IsRequired();
            builder.Property(d => d.ModifiedBy).IsRequired();

            builder.HasOne(s => s.Doctor)
                .WithMany(d => d.DoctorScheduleTemplates)
                .HasForeignKey(s => s.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
