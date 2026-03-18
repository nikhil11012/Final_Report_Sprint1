using Microsoft.EntityFrameworkCore;

namespace DOTNETCOREWEBAPI.Models
{
    public class EmpContext:DbContext
    {
        public EmpContext(DbContextOptions<EmpContext> options) : base(options)
        {
        }
        public DbSet<Department> departments { get; set; }
        public DbSet<Employees> Employees { get; set; }
        
    }
}
