public interface IHashingServices
{
    public string HashPassword(string password);
    public bool Verify(string HashedPassword, string InputPassword);

}