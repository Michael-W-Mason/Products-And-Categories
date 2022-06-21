#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Products_And_Categories.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    public string Name { get; set; }
    public List<Association> Products { get; set; } = new List<Association>();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}