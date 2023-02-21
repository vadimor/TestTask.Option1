using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Repositories.Interfaces
{
    // This interface provides methods for working with the Device table
    public interface IDeviceRepository
    {

        // This method adds a device to the table
        public Task<Device> AddDeviceAsync(string deviceToken);

        // This method retrieves a device by its device token
        public Task<Device?> GetDeviceAsync(string deviceToken);
    }
}
