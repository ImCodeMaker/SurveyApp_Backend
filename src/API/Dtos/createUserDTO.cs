using System.Text.Json.Serialization;

public class createUserDTO
{
    public string Email { get; set; } = null!;
    public string Password_Hash { get; set; } = null!;
    public string? Name { get; set; }
    public string? LastName { get; set; }

}