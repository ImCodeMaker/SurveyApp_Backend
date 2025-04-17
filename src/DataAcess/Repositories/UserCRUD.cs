using System.Data;
using Microsoft.EntityFrameworkCore;
public class UserRepository : IUser
{
    private readonly AppDbContext? appDbContext;
    private readonly IHashingServices? _hashingMethod;
    private readonly IUserFactory _userFactory;

    public UserRepository(AppDbContext _appDbContext,IHashingServices? hashingMethod, IUserFactory userFactory )
    {
        appDbContext = _appDbContext;
        _hashingMethod = hashingMethod;
        _userFactory = userFactory;
    }

    public async Task<User> createUser(createUserDTO user)
    {
        try
        {
            var hashPassCode = _hashingMethod!.HashPassword(user.Password_Hash);

             User newAdmin = _userFactory.CreateAdminUser(user.Email, user.Password_Hash, user.Name!, user.LastName!);
            
            var addedEntity = await appDbContext!.Users.AddAsync(newAdmin);
            await appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public List<User> getUsers()
    {
        var currentUsers = appDbContext!.Users.ToList();

        if (!currentUsers.Any())
        {
            return new List<User>();
        }

        return currentUsers;
    }

    public async Task deleteUser(int id)
    {
        try
        {
            var userToFind = await appDbContext!.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (userToFind == null)
            {
                throw new NullReferenceException("No user found");
            }

            appDbContext.Users.Remove(userToFind);
            await appDbContext.SaveChangesAsync();
        }
        catch (Exception)
        {

            throw;
        }

    }

    public async Task updateUser(User users)
    {
        try
        {
            var currentUser = await appDbContext!.Users.FirstOrDefaultAsync(user => user.Id == users.Id);

            if (currentUser != null)
            {
                currentUser.Name = users.Name;
                currentUser.LastName = users.LastName;
                currentUser.Email = users.Email;
                currentUser.Password_Hash = users.Password_Hash;

                var findforDuplicates = await appDbContext!.Users.FirstOrDefaultAsync(user => user.Name == users.Name || user.LastName == users.LastName || user.Email == users.Email);

                if (findforDuplicates != null)
                {
                    throw new DuplicateNameException("Ese nombre u correo ya existen, intenta con otro usuario!");
                }
            }
            await appDbContext.SaveChangesAsync();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<User> getUserID(int Id)
    {
        try
        {
            var user = await appDbContext!.Users.FirstOrDefaultAsync(user => user.Id == Id);

            if (user == null)
            {
                throw new Exception("No se encontro el usuario");
            }
            return await Task.FromResult(user);
        }
        catch (Exception)
        {

            throw;
        }
    }
}