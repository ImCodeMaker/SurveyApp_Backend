using Microsoft.EntityFrameworkCore;

public class HandleLogin : ILogin
{
    private readonly AppDbContext? _appdbcontext;
    private readonly IUser? _userRepository;
    private readonly IHashingServices? _hashingMethod;

    private readonly IUserFactory _userFactory;

    public HandleLogin(IUser userRepository, IHashingServices hashingMethod, AppDbContext? appDbContext, IUserFactory userFactory)
    {
        _userRepository = userRepository;
        _hashingMethod = hashingMethod;
        _appdbcontext = appDbContext;
        _userFactory = userFactory;
    }

    public async Task<User> SignUpHandler(createUserDTO user)
    {
        try
        {
            //Firstly we need to hash the password

            var HashedPassword = _hashingMethod!.HashPassword(user.Password_Hash);


            User newUser = _userFactory.CreateNormalUser(user.Email, user.Password_Hash, user.Name!, user.LastName!);
            var addedEntity = await _appdbcontext!.Users.AddAsync(newUser);
            await _appdbcontext!.SaveChangesAsync();
            return addedEntity.Entity;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<LoginResponse> LoginHandler(User userRequest)
    {
        try
        {
            var user = await _appdbcontext!.Users
                .FirstOrDefaultAsync(u => u.Email == userRequest.Email);


            if (user == null || !_hashingMethod!.Verify(user.Password_Hash, userRequest.Password_Hash))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Email o contraseña incorrectos"
                };
            }


            if (!UserSessionManager.Instance.TryLogin(user))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = user.IsAdmin
                        ? "Ya hay un administrador activo. Solo puede haber uno a la vez."
                        : "Este usuario ya tiene una sesión activa"
                };
            }


            return new LoginResponse
            {
                Success = true,
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                IsAdmin = user.IsAdmin,
                Message = "Inicio de sesión exitoso",
            };
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Login error: {ex.Message}");
            return new LoginResponse
            {
                Success = false,
                Message = "Error interno durante el inicio de sesión"
            };
        }
    }


    public async Task<LogoutResult> LogoutHandler(int userId)
    {
        try
        {

            if (userId <= 0)
                return new LogoutResult
                {
                    Success = false,
                    Message = "Invalid user ID",
                    WasActuallyLoggedIn = false
                };


            var user = await _appdbcontext!.Users.FindAsync(userId);
            if (user == null)
                return new LogoutResult
                {
                    Success = false,
                    Message = "User not found",
                    WasActuallyLoggedIn = false
                };


            var sessionManager = UserSessionManager.Instance;
            bool wasLoggedIn = sessionManager.IsUserLoggedIn(userId);


            if (!wasLoggedIn)
                return new LogoutResult
                {
                    Success = true, // Still "success" operation
                    Message = "User was not logged in",
                    WasActuallyLoggedIn = false
                };


            bool logoutSuccess = sessionManager.Logout(userId);

            return new LogoutResult
            {
                Success = logoutSuccess,
                Message = logoutSuccess ? "Logout successful" : "Logout failed",
                WasActuallyLoggedIn = true
            };
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Logout error: {ex.Message}");
            return new LogoutResult
            {
                Success = false,
                Message = $"Logout error: {ex.Message}",
                WasActuallyLoggedIn = false
            };
        }
    }
}
