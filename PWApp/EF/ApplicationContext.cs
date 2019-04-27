using Microsoft.EntityFrameworkCore;

namespace PWApp.EF
{
    public class ApplicationContext : DbContext
    {
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            Database.EnsureCreated();
           
        }
              
    }
}