using Microsoft.AspNetCore.Identity.Data;

public interface IUserActions
{
    Task<User> HandleSignUp(createUserDTO user);
    Task<LoginResponse> HandleLogin(User user);
    Task<LogoutResult> HandleLogout(int userId);
    
}