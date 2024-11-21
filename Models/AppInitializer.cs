namespace eBookStore.Models;

public class AppInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

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

            if (context.Cart.Any())
            {
                context.Cart.RemoveRange(context.Cart);
                context.SaveChanges();
            }

            if (context.Set<OrderBook>().Any())
            {
                context.Set<OrderBook>().RemoveRange(context.Set<OrderBook>());
                context.SaveChanges();
            }

            // Seed Books
            var books = new List<Book>
            {
                new Book { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Category = "Fiction", Price = 10.99m, ImageURL = "https://example.com/gatsby.jpg", Stock = 20 },
                new Book { Title = "1984", Author = "George Orwell", Category = "Dystopian", Price = 12.99m, ImageURL = "https://example.com/1984.jpg", Stock = 15 },
                new Book { Title = "To Kill a Mockingbird", Author = "Harper Lee", Category = "Classic", Price = 8.99m, ImageURL = "https://example.com/mockingbird.jpg", Stock = 10 },
                new Book { Title = "Moby Dick", Author = "Herman Melville", Category = "Adventure", Price = 14.99m, ImageURL = "https://example.com/mobydick.jpg", Stock = 12 },
                new Book { Title = "Pride and Prejudice", Author = "Jane Austen", Category = "Romance", Price = 9.99m, ImageURL = "https://example.com/pride.jpg", Stock = 18 },
                new Book { Title = "The Hobbit", Author = "J.R.R. Tolkien", Category = "Fantasy", Price = 11.99m, ImageURL = "https://example.com/hobbit.jpg", Stock = 25 }
            };
            context.Book.AddRange(books);
            context.SaveChanges();

            // Seed Users
            var users = new List<User>
            {
                new User { Name = "John Doe", Password = "password123", Email = "johndoe@example.com", Role = "Customer" },
                new User { Name = "Admin User", Password = "admin123", Email = "admin@example.com", Role = "Admin" },
                new User { Name = "Jane Smith", Password = "jane123", Email = "janesmith@example.com", Role = "Customer" },
                new User { Name = "Robert Brown", Password = "robert123", Email = "robertbrown@example.com", Role = "Customer" }
            };
            context.User.AddRange(users);
            context.SaveChanges();

            // Seed Orders
            var orders = new List<Order>
            {
                new Order { UserID = users[0].UserID, OrderDate = DateTime.Now, TotalPrice = books[0].Price, status = "Pending" },
                new Order { UserID = users[0].UserID, OrderDate = DateTime.Now.AddDays(-2), TotalPrice = books[1].Price * 2, status = "Completed" },
                new Order { UserID = users[2].UserID, OrderDate = DateTime.Now.AddDays(-1), TotalPrice = books[2].Price * 3, status = "Shipped" },
                new Order { UserID = users[3].UserID, OrderDate = DateTime.Now.AddDays(-3), TotalPrice = books[4].Price, status = "Pending" }
            };
            context.Order.AddRange(orders);
            context.SaveChanges();

            // Seed Feedback
            var feedbacks = new List<FeedBack>
            {
                new FeedBack { UserName = users[0].Name, BookID = books[0].BookID, Comment = "Amazing book! Highly recommend." },
                new FeedBack { UserName = users[0].Name, BookID = books[1].BookID, Comment = "Thought-provoking and very relevant." },
                new FeedBack { UserName = users[2].Name, BookID = books[2].BookID, Comment = "A timeless classic." },
                new FeedBack { UserName = users[3].Name, BookID = books[3].BookID, Comment = "Incredible story with deep meaning." }
            };
            context.FeedBack.AddRange(feedbacks);
            context.SaveChanges();

            // Seed Cart Items
            var cartItems = new List<Cart>
            {
                new Cart
                {
                    UserID = users[0].UserID,
                    BookID = books[0].BookID,
                    Title = books[0].Title,
                    Author = books[0].Author,
                    ImageURL = books[0].ImageURL,
                    Price = books[0].Price,
                    Quantity = 1
                },
                new Cart
                {
                    UserID = users[0].UserID,
                    BookID = books[1].BookID,
                    Title = books[1].Title,
                    Author = books[1].Author,
                    ImageURL = books[1].ImageURL,
                    Price = books[1].Price,
                    Quantity = 2
                },
                new Cart
                {
                    UserID = users[2].UserID,
                    BookID = books[3].BookID,
                    Title = books[3].Title,
                    Author = books[3].Author,
                    ImageURL = books[3].ImageURL,
                    Price = books[3].Price,
                    Quantity = 1
                },
                new Cart
                {
                    UserID = users[3].UserID,
                    BookID = books[4].BookID,
                    Title = books[4].Title,
                    Author = books[4].Author,
                    ImageURL = books[4].ImageURL,
                    Price = books[4].Price,
                    Quantity = 1
                }
            };
            context.Cart.AddRange(cartItems);
            context.SaveChanges();

            // Seed OrderBooks
            var orderBooks = new List<OrderBook>
            {
                new OrderBook { OrderID = orders[0].OrderID, BookID = books[0].BookID, Quantity = 1 },
                new OrderBook { OrderID = orders[1].OrderID, BookID = books[1].BookID, Quantity = 2 },
                new OrderBook { OrderID = orders[2].OrderID, BookID = books[2].BookID, Quantity = 3 },
                new OrderBook { OrderID = orders[3].OrderID, BookID = books[4].BookID, Quantity = 1 }
            };
            context.Set<OrderBook>().AddRange(orderBooks);
            context.SaveChanges();
        }
    }
}
