using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Repositories.Interfaces
{
    // This interface provides methods for working with the ExperimentValue table

    public interface IExperimentValueRepository
    {
        // This method retrieves values by their experiment ID
        public Task<IReadOnlyCollection<ExperimentValue>?> GetValuesAsync(int experimentId);
    }
}
