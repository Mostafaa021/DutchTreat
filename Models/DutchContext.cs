using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Models
{
    public class DutchContext : IdentityDbContext<StoreUser>
    {
        public DutchContext() : base()
        {

        }
        public DutchContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=DutchTreat;Integrated Security=True;") ;
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasData(new Order()
                {
                    Id = 1 , 
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "12356"
                });
            base.OnModelCreating(modelBuilder); // to handle IdentityUserLogin<String> Error
        }
    }
}
