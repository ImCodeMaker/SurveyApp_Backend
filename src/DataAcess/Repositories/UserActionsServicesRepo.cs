public class UserActionsServices : IUserActions
{
    private readonly ILogin _login;

    public UserActionsServices(ILogin login)
    {
        _login = login;
    }

    public async Task<User> HandleSignUp(createUserDTO user)
    {
       return await _login.SignUpHandler(user);
    }

    public async Task<LoginResponse> HandleLogin(User user)
    {
       return await _login.LoginHandler(user);
    }

    public async Task<LogoutResult> HandleLogout(int userId)
    {
        return await _login.LogoutHandler(userId);
    }
}