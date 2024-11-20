using Microsoft.EntityFrameworkCore;

namespace eBookStore.Models;

public class AppDbContex : DbContext
{
    public AppDbContex(DbContextOptions<AppDbContex> options) : base(options)
    {

    }

    public DbSet<Book> Book { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<FeedBack> FeedBack { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<OrderBook> OrderBook { get; set; }
    public DbSet<Cart> Carts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderBook>()
            .HasKey(ob => new { ob.OrderID, ob.BookID });

        modelBuilder.Entity<Cart>()
            .HasKey(c => new { c.UserID, c.BookID });
    }
}
