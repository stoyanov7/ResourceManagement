namespace ResourceManagement.Data
{
    using Microsoft.EntityFrameworkCore;
    using ResourceManagement.Models;

    public class ResourceManagementContext : DbContext
    {
        public ResourceManagementContext(DbContextOptions<ResourceManagementContext> options) 
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<PersonType> PersonTypes { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonPersonType>()
                .HasKey(pt => new { pt.PersonId, pt.PersonTypeId });

            modelBuilder.Entity<PersonPersonType>()
                .HasOne(pt => pt.Person)
                .WithMany(p => p.PersonPersonType)
                .HasForeignKey(pt => pt.PersonId);

            modelBuilder.Entity<PersonPersonType>()
                .HasOne(pt => pt.PersonType)
                .WithMany(t => t.PersonPersonTypes)
                .HasForeignKey(pt => pt.PersonTypeId);
        }
    }
}
