namespace ResourceManagement.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public IList<ScheduleItem> ScheduleItems { get; set; }

        public IList<RecurringSchedule> RecurringSchedules { get; set; }

        public IList<ScheduleException> ScheduleExceptions { get; set; }
    }
}
