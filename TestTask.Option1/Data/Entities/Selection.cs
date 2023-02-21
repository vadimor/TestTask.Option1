namespace TestTask.Option1.Data.Entities
{
    public class Selection
    {
        public int Id { get; set; }

        public int ExperimentValueId { get; set; }

        public ExperimentValue ExperimentValue { get; set; } = null!;

        public int DeviceId { get; set; }

        public Device Device { get; set; } = null!;
    }
}
