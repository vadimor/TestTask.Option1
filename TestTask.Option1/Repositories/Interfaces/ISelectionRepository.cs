using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Repositories.Interfaces
{

    // This interface provides methods for working with the Selection table

    public interface ISelectionRepository
    {
        // This method retrieves a value by device ID and experiment ID
        public Task<ExperimentValue?> GetValueAsync(int deviceId, int experimentId);

        // This method creates a new selection
        public Task<Selection?> AddSelectionAsync(int deviceId, int valueId);

        // This method returns the count of devices with the specified value ID
        public Task<int> GetCountDevicesByValueId(int valueId);
    }
}
