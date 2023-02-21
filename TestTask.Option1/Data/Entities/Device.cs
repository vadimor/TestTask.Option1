namespace TestTask.Option1.Data.Entities
{
    public class Device
    {
        public int Id { get; set; }

        public string DeviceToken { get; set; } = null!;

        public DateTime CreateDate { get; set; }
    }
}