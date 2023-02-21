using AutoMapper;
using TestTask.Option1.Data;
using TestTask.Option1.Data.Entities;
using TestTask.Option1.Data.Interfaces;
using TestTask.Option1.Helper.Interfaces;
using TestTask.Option1.Models.Dtos;
using TestTask.Option1.Models.Response;
using TestTask.Option1.Repositories.Interfaces;
using TestTask.Option1.Services.Interfaces;

namespace TestTask.Option1.Services
{
    public class ExperimentManageService : BaseDataService<ApplicationDbContext>, IExperimentManageService
    {
        private readonly ILogger<ExperimentManageService> _logger;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IExperimentRepository _experimentRepository;
        private readonly ISelectionRepository _selectionRepository;
        private readonly IExperimentValueRepository _experimentValueRepository;
        private readonly IMapper _mapper;
        private readonly IRandomWrapper _random;

        public ExperimentManageService(
            ILogger<BaseDataService<ApplicationDbContext>> loggerBaseService,
            IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
            ILogger<ExperimentManageService> loggerExperimentService,
            IDeviceRepository deviceRepository,
            IExperimentRepository experimentRepository,
            IExperimentValueRepository experimentValueRepository,
            ISelectionRepository selectionRepository,
            IMapper mapper,
            IRandomWrapper randomWrapper
            ) : base(loggerBaseService, dbContextWrapper)
        {
            _logger = loggerExperimentService;
            _deviceRepository = deviceRepository;
            _experimentRepository = experimentRepository;
            _experimentValueRepository = experimentValueRepository;
            _selectionRepository = selectionRepository;
            _mapper = mapper;
            _random = randomWrapper;
        }

        public async Task<ExperimentValueDto?> GetExperimentValueAsync(string experimentName, string deviceToken)
        {
            var device = await RegisterDeviceAsync(deviceToken); // If the device already exists, it will return the existing device
            var experiment = await _experimentRepository.GetExperimentByNameAsync(experimentName); 

            if (!AccessCheck(experiment, device)) // If the experiment started after creating the device, the method will return false
            {
                return null;
            }

            var experimentValue = await _selectionRepository.GetValueAsync(device.Id, experiment!.Id);

            if (experimentValue is not null)
            {
                return _mapper.Map<ExperimentValueDto>(experimentValue);
            }

            var result = await CreateSelectionAsync(device.Id, experiment.Id);

            if (result is null)
            {
                throw new NullReferenceException("Creating selection returned null");
            }

            return _mapper.Map<ExperimentValueDto>(result.ExperimentValue);
        }

        public async Task<IReadOnlyCollection<ExperimentStatisticResponse>> GetStaticticsAsync()
        {
            var list = new List<ExperimentStatisticResponse>();
            var experiments = await _experimentRepository.GetExperimentsAsync();

            if (experiments is null)
            {
                return new List<ExperimentStatisticResponse>();
            }

            foreach (var experiment in experiments)
            {
                var item = new ExperimentStatisticResponse
                {
                    ExperimentName = experiment.Name,
                    ValueStatistic = new Dictionary<string, int>()
                };

                var values = await _experimentValueRepository.GetValuesAsync(experiment.Id);

                if (values is null)
                {
                    continue;
                }

                foreach (var value in values)
                {
                    item.ValueStatistic.Add(value.Value, await _selectionRepository.GetCountDevicesByValueId(value.Id));
                }
                list.Add(item);
            }

            return list;
        }

        private async Task<Device> RegisterDeviceAsync(string deviceToken)
        {
            var device = await _deviceRepository.GetDeviceAsync(deviceToken);

            if (device is not null)
            {
                return device;
            }

            return await ExecuteSafeAsync(async () =>
            {
                return await _deviceRepository.AddDeviceAsync(deviceToken);
            });
        }

        private bool AccessCheck(Experiment? experiment, Device device)
        {
            if (experiment is null)
            {
                return false;
            }

            if (experiment.StartTime > device.CreateDate)
            {
                return false;
            }

            return true;
        }

        private async Task<Selection?> CreateSelectionAsync(int deviceId, int experimentId)
        {
            var values = await _experimentValueRepository.GetValuesAsync(experimentId);

            if (values is null)
            {
                return null;
            }

            var chanseCount = 0f;

            foreach (var value in values) // It sums all chanse. So we get какое-то относительное число that well be "100%" 
            {
                chanseCount += value.Chanse;
            }


            var randomChanse = _random.NextSingle() * chanseCount; // Getting some a percent between "0%" and "100%"

            var chanseStep = 0f;

            var selectedValue = values.First();

            foreach (var value in values) // Finding the value that matches withІ the received percent
            {
                chanseStep += value.Chanse;

                if (randomChanse <= chanseStep)
                {
                    selectedValue = value;
                    break;
                }
            }

            return await ExecuteSafeAsync(async () =>
            {
                return await _selectionRepository.AddSelectionAsync(deviceId, selectedValue.Id);
            });
        }
    }
}
