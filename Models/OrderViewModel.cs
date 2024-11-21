namespace eBookStore.Models;

public class OrderViewModel
{
    public Dictionary<int, Book>? Books { get; set; }
    public IEnumerable<OrderBook>? OrderedBooks { get; set; }
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string status { get; set; }
}
