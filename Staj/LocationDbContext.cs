using Microsoft.EntityFrameworkCore;

namespace Staj
{
    public class LocationDbContext:DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options): base(options) { }

        public DbSet<Location> locations { get; set; }  

    }
}
