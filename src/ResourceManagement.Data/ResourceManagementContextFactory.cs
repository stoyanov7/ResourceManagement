namespace ResourceManagement.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class ResourceManagementContextFactory : IDesignTimeDbContextFactory<ResourceManagementContext>
    {
        public ResourceManagementContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResourceManagementContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Database=ResourceManagement;Username=postgres;Password=password");

            return new ResourceManagementContext(optionsBuilder.Options);
        }
    }
}
