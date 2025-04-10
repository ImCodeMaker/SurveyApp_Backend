using Microsoft.EntityFrameworkCore;

public class HandleLogin : ILogin
{
    private readonly AppDbContext? _appdbcontext;
    private readonly IUser? _userRepository;
    private readonly IHashingServices? _hashingMethod;

    public HandleLogin(IUser userRepository, IHashingServices hashingMethod, AppDbContext? appDbContext)
    {
        _userRepository = userRepository;
        _hashingMethod = hashingMethod;
        _appdbcontext = appDbContext;
    }

    public async Task<User> SignUpHandler(User user)
    {
        try
        {
            //Firstly we need to hash the password

            var HashedPassword = _hashingMethod!.HashPassword(user.Password_Hash);


            var newUser = new User
            {
                Email = user.Email,
                Password_Hash = HashedPassword,
                Name = user.Name,
                LastName = user.LastName,
            };
            var createdUser = await _userRepository!.createUser(newUser);
            return createdUser;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<User> LoginHandler(User userRequest)
    {
        var findUser = await _appdbcontext!.Users
        .FirstOrDefaultAsync(user => user.Email == userRequest.Email);

        if (findUser == null)
        {
            return null!;
        }

        var passwordValid = _hashingMethod!.Verify(
            findUser.Password_Hash,
            userRequest.Password_Hash);

        return passwordValid ? findUser : null!;

    }
}