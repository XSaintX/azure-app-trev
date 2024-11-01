using Microsoft.EntityFrameworkCore;

namespace azure_app_trev.wwwroot.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        { 

        }

        public DbSet<Person> Persons { get; set; }
    }
}
