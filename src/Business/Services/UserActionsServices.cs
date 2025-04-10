public class UserActionsServices : IUserActions
{
    private readonly ILogin _login;

    public UserActionsServices(ILogin login)
    {
        _login = login;
    }

    public async Task<User> HandleSignUp(User user)
    {
       return await _login.SignUpHandler(user);
    }

    public async Task<User> HandleLogin(User user)
    {
       return await _login.LoginHandler(user);
    }
}