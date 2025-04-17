public class LogoutResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public bool WasActuallyLoggedIn { get; set; }
}