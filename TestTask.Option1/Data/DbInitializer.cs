using Microsoft.EntityFrameworkCore;
using TestTask.Option1.Data.Entities;

namespace TestTask.Option1.Data
{
    public class DbInitializer
    {
        // This class will create and initialize the databese, if it doesn`t exist 

        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            bool save = false;  // This allows all changes to be saved with a single request

            var a = context.Database.GetConnectionString();

            try
            {
                var b = context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var c = ex.Message;
            }

            if (!context.Devices.Any())
            {
                await context.Devices.AddRangeAsync(GetPreconfiguredDevice());
                save = true;
            }

            if (!context.Experiments.Any())
            {
                await context.Experiments.AddRangeAsync(GetPreconfiguredExperiment());
                save = true;
            }

            if (!context.ExperimentValues.Any())
            {
                await context.ExperimentValues.AddRangeAsync(GetPreconfiguredExperimentValue());
                save = true;
            }

            if (save)
            {
                await context.SaveChangesAsync();
            }


        }

        static public IEnumerable<Device> GetPreconfiguredDevice()
        {
            return new List<Device>()
            {
                new Device { DeviceToken = "User1", CreateDate = new DateTime(2023, 2, 16, 7, 23, 0)},
                new Device { DeviceToken = "User2", CreateDate = new DateTime(2023, 2, 15, 13, 12, 0)},
                new Device { DeviceToken = "User3", CreateDate = new DateTime(2023, 2, 16, 2, 11, 14)},
                new Device { DeviceToken = "User4", CreateDate = new DateTime(2023, 2, 13, 8, 3, 14)},
                new Device { DeviceToken = "User5", CreateDate = new DateTime(2023, 2, 11, 6, 44, 14)},
                new Device { DeviceToken = "User6", CreateDate = new DateTime(2023, 2, 5, 12, 2, 14)},
                new Device { DeviceToken = "User7", CreateDate = new DateTime(2023, 1, 1, 23, 32, 14)},
                new Device { DeviceToken = "User8", CreateDate = new DateTime(2023, 1, 24, 1, 33, 14)}
            };
        }

        static public IEnumerable<Experiment> GetPreconfiguredExperiment()
        {
            return new List<Experiment>()
            {
                new Experiment { Name = "button-color", StartTime = new DateTime(2023, 2, 16)},
                new Experiment { Name = "price-change", StartTime = new DateTime(2023, 2, 16)}
            };
        }

        static public IEnumerable<ExperimentValue> GetPreconfiguredExperimentValue()
        {
            return new List<ExperimentValue>()
            {
                new ExperimentValue {ExperimentId = 1, Value ="#FF0000", Chanse = 3.33f},
                new ExperimentValue {ExperimentId = 1, Value ="#00FF00", Chanse = 3.33f},
                new ExperimentValue {ExperimentId = 1, Value ="#0000FF", Chanse = 3.33f},
                new ExperimentValue {ExperimentId = 2, Value ="5", Chanse = 10},
                new ExperimentValue {ExperimentId = 2, Value ="10", Chanse = 75},
                new ExperimentValue {ExperimentId = 2, Value ="20", Chanse = 10},
                new ExperimentValue {ExperimentId = 2, Value ="50", Chanse = 5 }
            };
        }
    }
}
