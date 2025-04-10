using Microsoft.AspNetCore.Identity.Data;

public interface IUserActions
{
    Task<User> HandleSignUp(User user);
    Task<User> HandleLogin(User user);
    
}