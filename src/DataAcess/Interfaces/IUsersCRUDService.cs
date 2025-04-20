public interface UsersCRUDService
{
    Task<User> setUser(createUserDTO user);
    List<User> GetUsers();
    Task<User> GetUsersID(int Id);
    Task deleteUser(int Id);
    Task updateUser(User user);
}