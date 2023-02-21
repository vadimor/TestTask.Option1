namespace TestTask.Option1.Models.Dtos
{
    public class ExperimentValueDto
    {

        public int Id { get; set; }

        public string Value { get; set; } = null!;

        public float Chanse { get; set; }

        public ExperimentDto Experiment { get; set; } = null!;
    }
}
