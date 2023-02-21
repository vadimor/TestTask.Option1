namespace TestTask.Option1.Models.Dtos
{
    public class SelectionDto
    {
        public int Id { get; set; }

        public ExperimentValueDto experimentValue { get; set; } = null!;

        public DeviceDto Device { get; set; } = null!;
    }
}
