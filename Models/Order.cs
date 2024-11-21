using System.ComponentModel.DataAnnotations;

namespace eBookStore.Models;

public class Order
{
    [Key]
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string status { get; set; }
}
