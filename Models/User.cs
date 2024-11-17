using System.ComponentModel.DataAnnotations;

namespace eBookStore.Models;

public class User
{
    [Key]
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Role { get; set; }
}
