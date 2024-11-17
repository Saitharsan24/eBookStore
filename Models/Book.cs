using System.ComponentModel.DataAnnotations;

namespace eBookStore.Models;

public class Book
{
    [Key]
    public int BookID { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Author { get; set; }
    [Required]
    public string Category { get; set; }
    public decimal Price { get; set; }
    public string ImageURL { get; set; }
    public int Stock { get; set; }
}
