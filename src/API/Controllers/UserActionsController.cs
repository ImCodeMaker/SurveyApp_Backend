using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserActionsController : ControllerBase
{
    private readonly IUserActions _userActions;

    public UserActionsController(IUserActions userActions)
    {
        _userActions = userActions;
    }


    [HttpPost("signup")]
    public async Task<IActionResult> SignUpHandler(createUserDTO createUserDTO)
    {
        if (string.IsNullOrEmpty(createUserDTO.Email) ||
            string.IsNullOrEmpty(createUserDTO.Password_Hash) ||
            string.IsNullOrEmpty(createUserDTO.Name) ||
            string.IsNullOrEmpty(createUserDTO.LastName))
        {
            return StatusCode(400, new { message = "You must complete all the fields to continue." });
        }

        try
        {
            var newUser = new User
            {
                Email = createUserDTO.Email,
                Password_Hash = createUserDTO.Password_Hash,
                Name = createUserDTO.Name,
                LastName = createUserDTO.LastName,
                Created_At = DateTime.Now
            };

            var createdUser = await _userActions.HandleSignUp(newUser);


            return StatusCode(201, createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Hubo un error al crear el usuario. Detalles: {ex.Message}" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginHandler([FromBody] LoginRequest loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
        {
            return BadRequest("Email and password are required");
        }

        try
        {
            var user = new User
            {
                Email = loginRequest.Email,
                Password_Hash = loginRequest.Password // Will be hashed and verified
            };

            var loggedInUser = await _userActions.HandleLogin(user);

            if (loggedInUser == null)
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok(loggedInUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred during login {ex}");
        }
    }

}
