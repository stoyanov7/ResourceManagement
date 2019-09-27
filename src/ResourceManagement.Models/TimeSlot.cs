namespace ResourceManagement.Models
{
    using System;

    public class TimeSlot
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public ScheduleStatus Status { get; set; }
    }
}
