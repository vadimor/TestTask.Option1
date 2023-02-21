namespace TestTask.Option1.Models.Dtos
{
    public class DeviceDto
    {
        public int Id { get; set; }

        public string DeviceToken { get; set; } = null!;

        public DateTime CreateDate { get; set; }
    }
}
