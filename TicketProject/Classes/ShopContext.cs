using Microsoft.EntityFrameworkCore;


namespace TicketProject.Classes
{
    public class ShopContext : DbContext
    {
        public DbSet<User> users { get; set; }

        public DbSet<Transactions> transaction { get; set; }

       public ShopContext(DbContextOptions options) : base(options) 
        {
        } 
    }
}
