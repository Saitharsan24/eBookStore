namespace eBookStore.Models;

public class Cart
{
    public int UserID { get; set; }
    public int BookID { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ImageURL { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

}
