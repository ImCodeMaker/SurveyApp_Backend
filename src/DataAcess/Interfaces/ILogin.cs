public interface ILogin
{
    Task<User> SignUpHandler(createUserDTO user);
    Task<LoginResponse> LoginHandler(User user);
    Task<LogoutResult> LogoutHandler(int userId);
}