using System.ComponentModel.DataAnnotations;

namespace LasPollasHermanas.Shared.Models;

public class Dildo
{
    // Annotations 
    public int Id { get; set; }
    [Required][StringLength(50)]
    public string? Name { get; set; }
    [Required][Range (1,100)]
    public decimal Price { get; set; }
    public decimal Size { get; set; }
    public DateTime ExpireDate { get; set; }
    public string? Material { get; set; }
    public string? Color { get; set; }
    public int Stock { get; set; }
}