using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Data.EntityConfigurations
{
    public class ExperimentValueEntityTypeConfiguration
        : IEntityTypeConfiguration<ExperimentValue>
    {
        public void Configure(EntityTypeBuilder<ExperimentValue> builder)
        {
            builder.ToTable("ExperimentValue");

            builder.Property(x => x.Id)
                .UseHiLo("experiment_value_hilo")
                .IsRequired();

            builder.Property(x => x.Value)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Chanse)
                .IsRequired();

            builder.HasOne(x => x.Experiment)
                .WithMany()
                .HasForeignKey(x => x.ExperimentId);
        }
    }
}
