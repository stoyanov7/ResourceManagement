﻿namespace ResourceManagement.Infrastructure
{
    using NCrontab;
    using ResourceManagement.Domain;
    using ResourceManagement.Domain.Logger;
    using ResourceManagement.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ScheduleService : IScheduleService
    {
        private readonly IMyLogger<ScheduleService> logger;

        public ScheduleService(IMyLogger<ScheduleService> logger) => this.logger = logger;

        public IEnumerable<TimeSlot> ExpandSchedule(Schedule schedule, DateTime startTime, DateTime endTime)
        {
            var outSchedule = new List<TimeSlot>();

            if (schedule.ScheduleItems != null)
            { 
                outSchedule.AddRange(ExpandScheduleItems(schedule.ScheduleItems, startTime, endTime));
            }

            if (schedule.RecurringSchedules != null)
            {
                outSchedule.AddRange(ExpandRecurringSchedules(schedule.RecurringSchedules, startTime, endTime));
            }

            return outSchedule;
        }

        private IEnumerable<TimeSlot> ExpandScheduleItems(IList<ScheduleItem> scheduleItems, DateTime startTime, DateTime endTime)
        {
            return scheduleItems
                .Where(i => i.StartDateTime < endTime && i.EndDateTime > startTime)
                .Select(i => new TimeSlot()
                {
                    StartDateTime = i.StartDateTime,
                    EndDateTime = i.EndDateTime,
                    Status = i.Status
                })
                .OrderBy(t => t.StartDateTime);
        }

        private IEnumerable<TimeSlot> ExpandRecurringSchedules(IList<RecurringSchedule> recurringSchedules, DateTime startTime, DateTime endTime)
        {
            var outList = new List<TimeSlot>();

            foreach (var r in recurringSchedules)
            {
                var expandRange = CalculateIntersectionOfTimeslots((startTime, endTime), (r.MinStartDateTime, r.MaxEndDateTime));

                if (expandRange == null)
                {
                    continue;
                }

                // Console.Out.WriteLine(expandRange.Value.ToString());

                var cts = CrontabSchedule.Parse(r.CronPattern);
                outList.AddRange(cts.GetNextOccurrences(expandRange.Value.begin, expandRange.Value.end)
                    .Select(d => new TimeSlot
                    {
                        StartDateTime = d,
                        EndDateTime = d.Add(r.Duration),
                        Status = r.Status
                    }));
            }

            return outList;
        }

        private (DateTime begin, DateTime end)? CalculateIntersectionOfTimeslots(
            (DateTime StartDateTime, DateTime EndDateTime) ts1,
            (DateTime StartDateTime, DateTime EndDateTime) ts2)
        {
            this.logger.Trace($"Checking Time 1 {ts1} against Time 2 {ts2}");

            // Eliminate scenario 1 and 2
            if (!(ts1.StartDateTime <= ts2.EndDateTime && ts2.StartDateTime <= ts1.EndDateTime))
            {
                this.logger.Trace("Did not find an intersection");
                return null;
            }

            // Scenario 3
            if (ts1.StartDateTime >= ts2.StartDateTime && ts1.EndDateTime <= ts2.EndDateTime)
            {
                this.logger.Trace("Time 2 is contained entirely within Time 1");
                return (ts1.StartDateTime, ts1.EndDateTime);
            }

            // Scenario 4
            if (ts1.StartDateTime <= ts2.StartDateTime && ts1.EndDateTime >= ts2.EndDateTime)
            {
                this.logger.Trace("Time 1 is contained entirely within Time 2");
                return (ts2.StartDateTime, ts2.EndDateTime);
            }

            // Scenario 5
            if (ts1.StartDateTime >= ts2.StartDateTime && ts1.EndDateTime >= ts2.EndDateTime)
            {
                this.logger.Trace("Time 1 starts after Time 2 and overlaps");
                return (ts1.StartDateTime, ts2.EndDateTime);
            }

            // Scenario 6
            if (ts1.StartDateTime <= ts2.StartDateTime && ts1.EndDateTime <= ts2.EndDateTime)
            {
                this.logger.Trace("Time 2 starts after Time 1 and overlaps");
                return (ts2.StartDateTime, ts1.EndDateTime);
            }

            this.logger.Trace("It broke");

            return null;
        }
    }
}
