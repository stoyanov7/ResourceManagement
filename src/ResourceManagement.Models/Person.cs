namespace ResourceManagement.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string SurName { get; set; }

        public string PhoneNumber { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public IList<PersonPersonType> PersonPersonType { get; set; }
    }
}
