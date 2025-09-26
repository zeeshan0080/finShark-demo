using finShark_demo.Models;
using Microsoft.EntityFrameworkCore;



namespace finShark_demo.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}