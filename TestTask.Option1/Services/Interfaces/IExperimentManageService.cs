using TestTask.Option1.Models.Dtos;
using TestTask.Option1.Models.Response;

namespace TestTask.Option1.Services.Interfaces
{
    // This interface provides methods for working with the experiment system

    public interface IExperimentManageService
    {
        // This method return one of several options for experiment values
        // One device token should always have the same experiment value for every request 
        public Task<ExperimentValueDto?> GetExperimentValueAsync(string experimentName, string deviceToken);

        // This method returns statistics about the results of experiments
        public Task<IReadOnlyCollection<ExperimentStatisticResponse>> GetStaticticsAsync();
    }
}
