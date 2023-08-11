using HR.LeaveManagement.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Persistence.Configurations
{
    public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            //Data Seeding in LeaveType Table
            builder.HasData(new LeaveType
            {
                Id = 1,
                Name = "Vacation",
                DefaultDays = 7,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
            });
            //Fluent Validation for LeaveType Table
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
