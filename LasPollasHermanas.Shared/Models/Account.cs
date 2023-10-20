namespace LasPollasHermanas.Shared.Models;

public class Account
{
    public int    Id       { get; set; }
    public string Email    { get; set; } = null!;
    public string Password { get; set; } = null!;

    public AdminUser? AdminUser { get; set; }
    public MortalUser? MortalUser { get; set; }
}