namespace ResourceManagement.Domain
{
    using System;
    using System.Collections.Generic;
    using ResourceManagement.Models;

    public interface IScheduleService
    {
        IEnumerable<TimeSlot> ExpandSchedule(Schedule schedule, DateTime startTime, DateTime endTime);
    }
}