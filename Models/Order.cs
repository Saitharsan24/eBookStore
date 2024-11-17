using System.ComponentModel.DataAnnotations;

namespace eBookStore.Models;

public class Order
{
    [Key]
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public int BookID { get; set; }
    public DateTime OrderDate { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
