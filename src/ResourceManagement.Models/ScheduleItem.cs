namespace ResourceManagement.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // <summary>
    /// A one-off instance of an item on the schedule
    /// </summary>
    public class ScheduleItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public ScheduleStatus Status { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public TimeSpan Duration => EndDateTime.Subtract(StartDateTime);
    }
}
