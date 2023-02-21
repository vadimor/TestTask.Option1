namespace TestTask.Option1.Models.Response
{
    public class ExperimentStatisticResponse
    {
        public string ExperimentName { get; set; } = null!;

        public IDictionary<string, int> ValueStatistic { get; set; } = new Dictionary<string, int>();
    }
}
