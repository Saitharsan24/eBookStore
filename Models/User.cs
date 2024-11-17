using System.ComponentModel.DataAnnotations;

namespace eBookStore.Models;

public class User
{
    [Key]
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    [Required]
    public string Email { get; set; }
    public string Role { get; set; }
}
