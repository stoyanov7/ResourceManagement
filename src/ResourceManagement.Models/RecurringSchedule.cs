namespace ResourceManagement.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
	/// An event that re-occurs on the schedule given a recurrence pattern defined in CronPattern
	/// </summary>
    public class RecurringSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string CronPattern { get; set; }        

        public ScheduleStatus Status { get; set; }

        public TimeSpan Duration { get; set; }

        public DateTime MinStartDateTime { get; set; }

        public DateTime MaxEndDateTime { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
    }
}
