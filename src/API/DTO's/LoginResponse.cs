public class LoginResponse
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public bool IsAdmin { get; set; }
    public string? Message { get; set; }
    public bool Success {get; set;}
}