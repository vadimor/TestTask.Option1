using Microsoft.EntityFrameworkCore;
using TestTask.Option1.Data;
using TestTask.Option1.Data.Entities;
using TestTask.Option1.Repositories.Interfaces;

namespace TestTask.Option1.Repositories
{
    // This class works with the database

    public class ApplicationRepository
        : IDeviceRepository, IExperimentRepository, IExperimentValueRepository, ISelectionRepository
    {
        private readonly ILogger<ApplicationRepository> _logger;
        private readonly ApplicationDbContext _dbContext;

        public ApplicationRepository(ILogger<ApplicationRepository> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        // Implementation of IDeviceRepository

        public async Task<Device> AddDeviceAsync(string deviceToken)
        {
            var device = await GetDeviceAsync(deviceToken);

            if (device is not null)
            {
                return device;
            }

            device = new Device
            {
                DeviceToken = deviceToken,
                CreateDate = DateTime.Now,
            };

            var result = await _dbContext.Devices.AddAsync(device);

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<Device?> GetDeviceAsync(string deviceToken)
        {
            return await _dbContext.Devices
                .FirstOrDefaultAsync(x => x.DeviceToken == deviceToken);
        }


        // Implementation of IExperementRepository

        public async Task<Experiment?> GetExperimentByNameAsync(string name)
        {
            return await _dbContext.Experiments
                .FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IReadOnlyCollection<Experiment>?> GetExperimentsAsync()
        {
            return await _dbContext.Experiments.ToListAsync();
        }


        // Implementation of IExperementValueRepository
        public async Task<IReadOnlyCollection<ExperimentValue>?> GetValuesAsync(int experimentId)
        {
            return await _dbContext.ExperimentValues
                .Where(x => x.ExperimentId == experimentId)
                .Include(x => x.Experiment)
                .ToListAsync();
        }


        // Implementation of ISelectionRepository

        public async Task<ExperimentValue?> GetValueAsync(int deviceId, int experimentId)
        {
            var result = await _dbContext.Selections
                .Where(x => x.DeviceId == deviceId && x.ExperimentValue.ExperimentId == experimentId) // optimizated 
                .Include(x => x.ExperimentValue)
                .ThenInclude(x => x.Experiment)
                .FirstOrDefaultAsync();


            return result is not null ? result.ExperimentValue : null;
        }

        public async Task<Selection?> AddSelectionAsync(int deviceId, int valueId)
        {
            var result = await _dbContext.Selections.AddAsync(
                    new Selection { DeviceId = deviceId, ExperimentValueId = valueId }
                );

            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        public async Task<int> GetCountDevicesByValueId(int valueId)
        {
            return await _dbContext.Selections.Where(x => x.ExperimentValueId == valueId).CountAsync();
        }
    }
}
