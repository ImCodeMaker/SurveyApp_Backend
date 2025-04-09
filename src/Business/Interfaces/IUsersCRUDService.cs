public interface UsersCRUDService
{
    Task setUser(User user);
    List<User> GetUsers();
    Task<User> GetUsersID(int Id);
    Task deleteUser(int Id);
    Task updateUser(User user);
}