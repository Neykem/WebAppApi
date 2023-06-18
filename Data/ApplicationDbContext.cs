using Microsoft.EntityFrameworkCore;
using WebAppApi.Data.DataModels;

namespace WebAppApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {

        }
        DbSet<Employee> Employees { get; set; }
    }
}
