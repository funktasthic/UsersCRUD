using Microsoft.EntityFrameworkCore;
using UsersCRUD.Models;

namespace UsersCRUD.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}