namespace Doctor.Domain.Entities
{
    public class SystemSetting : BaseEntity
    {
        public int SlotIntervalMinutes { get; set; } = 30; 
        public TimeSpan WorkStartTime { get; set; } = new TimeSpan(9, 0, 0); 
        public TimeSpan WorkEndTime { get; set; } = new TimeSpan(19, 0, 0); 
    }
}
