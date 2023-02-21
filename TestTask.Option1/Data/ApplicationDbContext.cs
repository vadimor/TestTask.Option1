using Microsoft.EntityFrameworkCore;
using TestTask.Option1.Data.Entities;
using TestTask.Option1.Data.EntityConfigurations;

namespace TestTask.Option1.Data
{

    // This class provides methods for working with database and it is responsible to modeling the database in code

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<ExperimentValue> ExperimentValues { get; set; }
        public DbSet<Selection> Selections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DeviceEntityTypeConfiguration());
            builder.ApplyConfiguration(new ExperimentEntityTypeConfiguration());
            builder.ApplyConfiguration(new ExperimentValueEntityTypeConfiguration());
            builder.ApplyConfiguration(new SelectionEntityTypeConfiguration());
        }
    }
}
