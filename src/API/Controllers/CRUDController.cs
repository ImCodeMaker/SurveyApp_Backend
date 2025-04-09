using Microsoft.AspNetCore.Mvc;

[ApiController]
public class CRUDController : ControllerBase
{
    private readonly UsersCRUDService? _userServices;

    public CRUDController(UsersCRUDService userServices)
    {
        _userServices = userServices;
    }

    [HttpGet("/users")]
    public IActionResult getUsers()
    {
        var users = _userServices!.GetUsers();
        return Ok(users);
    }
    [HttpGet("/users{Id}")]
    public async Task<IActionResult> getUserById(int Id)
    {
        try
        {
            var user = await _userServices!.GetUsersID(Id);
            return Ok(user);
        }
        catch (Exception ex)
        {

            return StatusCode(500, new {message = $"We couldn't find the user, please. try again! {ex}"});
        }
    }
    [HttpPost("/users")]
    public async Task<IActionResult> createUser(User user)
    {
        try
        {
            await _userServices!.setUser(user);
            return StatusCode(201, new { message = $"The user {user.Name} was successfully created" });
        }
        catch (Exception ex)
        {

            return StatusCode(500, new { message = "An error occurred while creating the user.", details = ex.Message });
        }
    }
    [HttpDelete("/user/{Id}")]
    public async Task<IActionResult> deleteUser(int Id)
    {
        try
        {
            await _userServices!.deleteUser(Id);

            return StatusCode(201, new {message = $"The user with the Id: {Id} was sucessfully deleted."});
        }
        catch (System.Exception ex)
        {
            
            return StatusCode(500, new {message = $"We're having an error trying to delete the user with the Id {Id}, Try to see if it really exists. {ex}"});
        }
    }

}



