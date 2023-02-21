using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Data.EntityConfigurations
{
    public class ExperimentEntityTypeConfiguration
        : IEntityTypeConfiguration<Experiment>
    {
        public void Configure(EntityTypeBuilder<Experiment> builder)
        {
            builder.ToTable("Experiment");

            builder.Property(x => x.Id)
                .UseHiLo("experiment_hilo")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();
        }
    }
}
