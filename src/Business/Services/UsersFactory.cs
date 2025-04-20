public class UserFactory : IUserFactory
{
    private readonly IHashingServices _hashingService;

    public UserFactory(IHashingServices hashingService)
    {
        _hashingService = hashingService;
    }

    public User CreateNormalUser(string email, string password, string name, string lastName)
    {
        return new User
        {
            Email = email,
            Password_Hash = _hashingService.HashPassword(password),
            Name = name,
            LastName = lastName,
            IsAdmin = false,
            Created_At = DateTime.UtcNow
        };
    }

    public User CreateAdminUser(string email, string password, string name, string lastName)
    {
        return new User
        {
            Email = email,
            Password_Hash = _hashingService.HashPassword(password),
            Name = name,
            LastName = lastName,
            IsAdmin = true,
            Created_At = DateTime.UtcNow
        };
    }
}