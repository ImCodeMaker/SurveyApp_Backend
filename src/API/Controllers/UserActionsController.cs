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
            

            var createdUser = await _userActions.HandleSignUp(createUserDTO);

            var response = new createdUserDTO{
                Id = createdUser.Id,
                isAdmin = createdUser.IsAdmin
            };


            return StatusCode(201, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Hubo un error al crear el usuario. Detalles: {ex.Message}" });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginHandler([FromBody] LoginRequests loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password_Hash))
        {
            return BadRequest("Email and password are required");
        }

        try
        {
            var user = new User
            {
                Email = loginRequest.Email,
                Password_Hash = loginRequest.Password_Hash // Will be hashed and verified
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

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] int userId)
    {
        try
        {
            var result = await _userActions.HandleLogout(userId);

            if (result == null)
                return NotFound("User not found.");

            return Ok(new
            {
                Success = true,
                Message = "Logout was sucessful."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Success = false,
                Message = $"Error: {ex.Message}"
            });
        }
    }

}
