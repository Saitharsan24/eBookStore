namespace eBookStore.Models;

public class AppInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContex>();

            context.Database.EnsureCreated();

            // Clear existing data
            if (context.Book.Any())
            {
                context.Book.RemoveRange(context.Book);
                context.SaveChanges();
            }

            if (context.User.Any())
            {
                context.User.RemoveRange(context.User);
                context.SaveChanges();
            }

            if (context.Order.Any())
            {
                context.Order.RemoveRange(context.Order);
                context.SaveChanges();
            }

            if (context.FeedBack.Any())
            {
                context.FeedBack.RemoveRange(context.FeedBack);
                context.SaveChanges();
            }

            // Seed Books
            var books = new List<Book>
            {
                new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Category = "Fiction", Price = 10.99m, ImageURL = "https://example.com/gatsby.jpg", Stock = 20 },
                new Book { Title = "1984", Author = "George Orwell", Category = "Dystopian", Price = 12.99m, ImageURL = "https://example.com/1984.jpg", Stock = 15 },
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Category = "Classic", Price = 8.99m, ImageURL = "https://example.com/mockingbird.jpg", Stock = 10 }
            };
            context.Book.AddRange(books);
            context.SaveChanges();

            // Seed Users
            var users = new List<User>
            {
                new User { Name = "John Doe", Password = "password123", Email = "johndoe@example.com", Role = "Customer" },
                new User { Name = "Admin User", Password = "admin123", Email = "admin@example.com", Role = "Admin" }
            };
            context.User.AddRange(users);
            context.SaveChanges();

            // Seed Orders
            var orders = new List<Order>
            {
                new Order { UserID = users[0].UserID, BookID = books[0].BookID, OrderDate = DateTime.Now, Quantity = 1, TotalPrice = books[0].Price },
                new Order { UserID = users[0].UserID, BookID = books[1].BookID, OrderDate = DateTime.Now.AddDays(-1), Quantity = 2, TotalPrice = books[1].Price * 2 }
            };
            context.Order.AddRange(orders);
            context.SaveChanges();

            // Seed Feedback
            var feedbacks = new List<FeedBack>
            {
                new FeedBack { UserID = users[0].UserID, BookID = books[0].BookID, Comment = "Amazing book! Highly recommend." },
                new FeedBack { UserID = users[0].UserID, BookID = books[1].BookID, Comment = "Thought-provoking and very relevant." }
            };
            context.FeedBack.AddRange(feedbacks);
            context.SaveChanges();
        }
    }
}
