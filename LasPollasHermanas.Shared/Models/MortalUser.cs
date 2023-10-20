namespace LasPollasHermanas.Shared.Models;

public class MortalUser
{
    public int      Id        { get; set; }
    public string   Name      { get; set; } = null!;
    public string   Surname   { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public List<Dildo>? CurrentDildos { get; set; }
    public List<Dildo>? BoughtDildos  { get; set; }

    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
}