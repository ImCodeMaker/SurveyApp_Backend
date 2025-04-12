using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AnswersController : ControllerBase
{
    private readonly IAnswerServices _answerServices;

    public AnswersController(IAnswerServices answerServices)
    {
        _answerServices = answerServices;
    }

    [HttpPost("/{userId}")]
    public async Task<IActionResult> setTask(int userId, [FromBody] CreateAnswerDTO createAnswerDTO)
    {
        if (createAnswerDTO == null || createAnswerDTO.Answers == null || !createAnswerDTO.Answers.Any())
        {
            return BadRequest("Invalid input data. Please provide answers.");
        }

        try
        {
            var result = await _answerServices.createAnswer(userId, createAnswerDTO);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            
            return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
        }
    }
}
