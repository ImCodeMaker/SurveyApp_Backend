public interface ILogin
{
    Task<User> SignUpHandler(User user);
    Task<User> LoginHandler(User user);
}