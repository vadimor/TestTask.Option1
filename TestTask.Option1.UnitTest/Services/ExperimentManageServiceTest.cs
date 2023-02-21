using AutoMapper;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using TestTask.Option1.Data;
using TestTask.Option1.Data.Entities;
using TestTask.Option1.Data.Interfaces;
using TestTask.Option1.Helper.Interfaces;
using TestTask.Option1.Models.Dtos;
using TestTask.Option1.Models.Response;
using TestTask.Option1.Repositories.Interfaces;
using TestTask.Option1.Services;
using TestTask.Option1.Services.Interfaces;
using Xunit;

namespace TestTask.Option1.UnitTest.Services
{
    public class ExperimentManageServiceTest
    {
        private readonly Mock<IDeviceRepository> _deviceRepository;
        private readonly Mock<IExperimentRepository> _experimentRepository;
        private readonly Mock<IExperimentValueRepository> _experimentValueRepository;
        private readonly Mock<ISelectionRepository> _selectionRepository;

        private readonly Mock<ILogger<ExperimentManageService>> _logger;
        private readonly Mock<ILogger<BaseDataService<ApplicationDbContext>>> _loggerBaseDataService;

        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
        private readonly Mock<IRandomWrapper> _random;

        private readonly IExperimentManageService _experimentManageService;

        public ExperimentManageServiceTest()
        {
            _deviceRepository = new Mock<IDeviceRepository>();
            _experimentRepository = new Mock<IExperimentRepository>();
            _experimentValueRepository = new Mock<IExperimentValueRepository>();
            _selectionRepository = new Mock<ISelectionRepository>();
            _logger = new Mock<ILogger<ExperimentManageService>>();
            _loggerBaseDataService = new Mock<ILogger<BaseDataService<ApplicationDbContext>>>();
            _mapper = new Mock<IMapper>();
            _random = new Mock<IRandomWrapper>();
            _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();

            var dbContextTransaction = new Mock<IDbContextTransaction>();
            _dbContextWrapper.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(dbContextTransaction.Object);



            _experimentManageService = new ExperimentManageService(
                _loggerBaseDataService.Object,
                _dbContextWrapper.Object,
                _logger.Object,
                _deviceRepository.Object,
                _experimentRepository.Object,
                _experimentValueRepository.Object,
                _selectionRepository.Object,
                _mapper.Object,
                _random.Object
                );
        }
        [Fact]
        public async Task GetStaticticsAsync_Success()
        {
            // arrange
            var experiments = new List<Experiment>
            {
                new()
                {
                    Id = 1,
                    Name = "Experiment1",
                },
                new()
                {
                    Id = 2,
                    Name = "Experiment2"
                }
            };

            _experimentRepository.Setup(
                x => x.GetExperimentsAsync()).ReturnsAsync(experiments);

            var firstValues = new List<ExperimentValue>
            {
                new()
                {
                    Id = 1,
                    ExperimentId = 1,
                    Value = "Value1"
                },
                new()
                {
                    Id = 2,
                    ExperimentId = 1,
                    Value = "Value2"
                },
                new()
                {
                    Id = 3,
                    ExperimentId = 1,
                    Value = "Value3"
                }
            };

            _experimentValueRepository.Setup(
                x => x.GetValuesAsync(It.Is<int>(x => x == 1))).ReturnsAsync(firstValues);

            var secondValues = new List<ExperimentValue>
            {
                new()
                {
                    Id = 4,
                    ExperimentId = 2,
                    Value = "Value4"
                },
                new()
                {
                    Id = 5,
                    ExperimentId = 2,
                    Value = "Value5"
                }
            };

            _experimentValueRepository.Setup(
                x => x.GetValuesAsync(It.Is<int>(x => x == 2))).ReturnsAsync(secondValues);

            _selectionRepository.Setup(
                x => x.GetCountDevicesByValueId(It.Is<int>(x => x == 1))).ReturnsAsync(1);
            _selectionRepository.Setup(
                x => x.GetCountDevicesByValueId(It.Is<int>(x => x == 2))).ReturnsAsync(2);
            _selectionRepository.Setup(
                x => x.GetCountDevicesByValueId(It.Is<int>(x => x == 3))).ReturnsAsync(3);
            _selectionRepository.Setup(
                x => x.GetCountDevicesByValueId(It.Is<int>(x => x == 4))).ReturnsAsync(4);
            _selectionRepository.Setup(
                x => x.GetCountDevicesByValueId(It.Is<int>(x => x == 5))).ReturnsAsync(5);

            var testResult = new List<ExperimentStatisticResponse>
            {
                new()
                {
                    ExperimentName = "Experiment1",
                    ValueStatistic = new Dictionary<string, int>
                    {
                        {"Value1", 1},
                        {"Value2", 2},
                        {"Value3", 3}
                    }
                },
                new()
                {
                    ExperimentName = "Experiment2",
                    ValueStatistic = new Dictionary<string, int>
                    {
                        {"Value4", 4},
                        {"Value5", 5}
                    }
                }
            };

            // act

            var result = await _experimentManageService.GetStaticticsAsync();

            // assert

            result.Should().NotBeNull();
            result.Count.Should().Be(2);

            var resultArray = result.ToArray();
            var testResultArray = testResult.ToArray();

            for (var i = 0; i < testResult.Count; i++)
            {
                resultArray[i].ExperimentName.Should().Be(testResult[i].ExperimentName);
                resultArray[i].ValueStatistic.Count.Should().Be(testResultArray[i].ValueStatistic.Count);
                foreach (var item in resultArray[i].ValueStatistic)
                {
                    if (!testResultArray[i].ValueStatistic.Contains(item))
                    {
                        Assert.Fail("ValueStatistic dosn`t contain KeyValuePair");
                    }
                }
            }
        }

        [Fact]
        public async Task GetExperimentValueAsync_Success()
        {
            var deviceToken = "Device1";
            var experimentName = "Experiment1";

            _deviceRepository.Setup(
                x => x.GetDeviceAsync(It.Is<string>(x => x.Equals(deviceToken))))
                .ReturnsAsync((Device?)null);



            var device = new Device
            {
                Id = 1,
                DeviceToken = "Device1",
                CreateDate = new DateTime(9, 9, 9)
            };

            _deviceRepository.Setup(
                x => x.AddDeviceAsync(It.Is<string>(x => x.Equals(deviceToken))))
                .ReturnsAsync(device);

            var experiment = new Experiment
            {
                Id = 1,
                Name = "Experiment1",
                StartTime = new DateTime(8, 8, 8)
            };

            _experimentRepository.Setup(
                x => x.GetExperimentByNameAsync(It.Is<string>(x => x.Equals(experimentName))))
                .ReturnsAsync(experiment);

            _selectionRepository.Setup(
                    x => x.GetValueAsync(It.Is<int>(x => x == 1), It.Is<int>(x => x == 1)))
                .ReturnsAsync((ExperimentValue?) null);

            var values = new List<ExperimentValue>
                    {
                        new()
                        {
                            Id = 1,
                            Chanse = 1,
                            Value = "Value1",
                            ExperimentId = 1,
                            Experiment = experiment
                        },
                        new()
                        {
                            Id = 2,
                            Chanse = 2,
                            Value = "Value2",
                            ExperimentId = 1,
                            Experiment = experiment
                        },
                        new()
                        {
                            Id = 3,
                            Chanse = 3,
                            Value = "Value3",
                            ExperimentId = 1,
                            Experiment = experiment
                        }
                    };

            _experimentValueRepository.Setup(
                    x => x.GetValuesAsync(It.Is<int>(x => x == 1)))
                .ReturnsAsync(values);

            _random.Setup(x => x.NextSingle()).Returns(0.45f);

            var selection = new Selection
            {
                Id = 1,
                DeviceId = 1,
                ExperimentValueId = 2,
                Device = device,
                ExperimentValue = new()
                {
                    Id = 2,
                    Chanse = 2,
                    Value = "Value2",
                    ExperimentId = 1,
                    Experiment = experiment
                }
            };

            _selectionRepository.Setup(
                x => x.AddSelectionAsync(It.Is<int>(x => x == 1), It.Is<int>(x => x == 2)))
                .ReturnsAsync(selection);

            var experimentValueDto = new ExperimentValueDto
            {
                Id = 2,
                Value = "Value2",
                Chanse = 2,
                Experiment = new ExperimentDto
                {
                    Id = 1,
                    Name = "Experiment1",
                    StartTime = new DateTime(8, 8, 8)
                }
            };

            _mapper.Setup(
                x => x.Map<ExperimentValueDto>(It.Is<ExperimentValue>(x =>
                    x.Id == 2
                    & x.Chanse == 2
                    & x.Value.Equals("Value2")
                    & x.Experiment.Id == 1
                    & x.Experiment.Name.Equals("Experiment1")
                    & x.Experiment.StartTime.Equals(new DateTime(8, 8, 8))
                ))).Returns(experimentValueDto);

            var result = await _experimentManageService.GetExperimentValueAsync(experimentName, deviceToken);
            
            result.Should().NotBeNull();

            if(!(experimentValueDto.Id == result!.Id
               & experimentValueDto.Value.Equals(result.Value)
               & experimentValueDto.Chanse== result.Chanse
               & experimentValueDto.Experiment.Id == result.Experiment.Id
               & experimentValueDto.Experiment.Name.Equals(result.Experiment.Name)
               & experimentValueDto.Experiment.StartTime.Equals(result.Experiment.StartTime)))
            {
                Assert.Fail("Result and expected aren`t same");
            }

        }
    }
}
