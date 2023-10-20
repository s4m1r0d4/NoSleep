namespace LasPollasHermanas.Shared;

public class MortalUserDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public int AccountId { get; set; }
}