using System.ComponentModel.DataAnnotations;

namespace eBookStore.Models;

public class FeedBack
{
    [Key]
    public int FeedbackID { get; set; }
    public int UserID { get; set; }
    public int BookID { get; set; }
    public string Comment { get; set; }
}
