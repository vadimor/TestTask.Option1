using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Data.EntityConfigurations
{
    public class SelectionEntityTypeConfiguration
        : IEntityTypeConfiguration<Selection>
    {
        public void Configure(EntityTypeBuilder<Selection> builder)
        {
            builder.ToTable("Selection");

            builder.Property(x => x.Id)
                .UseHiLo("selection_hilo")
                .IsRequired();

            builder.HasOne(x => x.Device)
                .WithMany()
                .HasForeignKey(x => x.DeviceId);

            builder.HasOne(x => x.ExperimentValue)
                .WithMany()
                .HasForeignKey(x => x.ExperimentValueId);
        }
    }
}
