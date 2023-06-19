using Microsoft.EntityFrameworkCore;
using WebAppApi.Data.DataModels;

namespace WebAppApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions contextOptions) : base(contextOptions)
        {

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
