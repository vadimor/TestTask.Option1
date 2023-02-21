using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Data.EntityConfigurations
{
    public class DeviceEntityTypeConfiguration
        : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.ToTable("Device");

            builder.Property(x => x.Id)
                .UseHiLo("device_hilo")
                .IsRequired();

            builder.Property(x => x.DeviceToken)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.CreateDate)
                .IsRequired();
        }
    }
}
