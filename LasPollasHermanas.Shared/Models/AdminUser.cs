namespace LasPollasHermanas.Shared.Models;

public class AdminUser
{
    public int    Id   { get; set; }
    public string Name { get; set; } = null!;

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
}