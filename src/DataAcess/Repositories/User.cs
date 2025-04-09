using System.Data;
using Microsoft.EntityFrameworkCore;
public class UserRepository : IUser
{
    private readonly AppDbContext? appDbContext;

    public UserRepository(AppDbContext _appDbContext)
    {
        appDbContext = _appDbContext;
    }

    public async Task createUser(User user)
    {
        try
        {
            await appDbContext!.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();
            
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