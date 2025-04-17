public interface IUserFactory
{
    User CreateNormalUser(string email, string password, string name, string lastName);
    User CreateAdminUser(string email, string password, string name, string lastName);
}