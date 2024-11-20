namespace eBookStore.Models;

public class BookViewModel
{
    public int BookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Category { get; set; }
    public decimal Price { get; set; }
    public string ImageURL { get; set; }
    public int Stock { get; set; }
    public IEnumerable<FeedBack>? FeedBacks { get; set; }
}
