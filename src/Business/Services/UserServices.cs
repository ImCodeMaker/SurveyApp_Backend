public class UserServices : UsersCRUDService
{
    private readonly IUser? _userRepository;

    public UserServices(IUser userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task setUser(User user)
    {
        try
        {
            await _userRepository!.createUser(user);
        }
        catch (Exception )
        {
            
            throw ;
        }
    }

    public List<User> GetUsers()
    {
        return _userRepository!.getUsers();
    }
    public async Task<User> GetUsersID(int Id)
    {
        return await _userRepository!.getUserID(Id);
    }

    public async Task deleteUser(int Id)
    {
        await _userRepository!.deleteUser(Id);
    }

    public async Task updateUser(User user)
    {
        await _userRepository!.updateUser(user);
    }
}