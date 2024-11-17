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

}
