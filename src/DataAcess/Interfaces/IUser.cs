public interface IUser
{
    Task<User> createUser(User user);
    List<User> getUsers();
    Task deleteUser(int id);
    Task updateUser(User user);
    Task<User> getUserID(int Id);
}