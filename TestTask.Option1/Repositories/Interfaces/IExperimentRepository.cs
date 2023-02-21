using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Repositories.Interfaces
{
    // This interface provides methods for working with the Experiment table

    public interface IExperimentRepository
    {
        // This method retrieves a experiment by its name
        public Task<Experiment?> GetExperimentByNameAsync(string name);

        // This method retrieves all experiments
        public Task<IReadOnlyCollection<Experiment>?> GetExperimentsAsync();
    }
}
