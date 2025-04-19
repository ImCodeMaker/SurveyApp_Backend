using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AnswersController : ControllerBase
{
    private readonly IAnswerServices _answerServices;
    private readonly AppDbContext _appDbContext;

    // Make sure this is the ONLY constructor in your controller
    public AnswersController(IAnswerServices answerServices, AppDbContext appDbContext)
    {
        _answerServices = answerServices;
        _appDbContext = appDbContext;
    }

    [HttpPost("answers/{userId}")]
    public async Task<IActionResult> SetAnswer(int userId, [FromBody] CreateAnswerDTO createAnswerDTO)
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

    [HttpGet("check-answer/{surveyId}/{userId}")]
    public async Task<IActionResult> CheckUserAnswer(int surveyId, int userId)
    {
        var hasAnswered = await _appDbContext.Answers
            .AnyAsync(a => a.Survey_Id == surveyId && a.User_Id == userId);

        return Ok(new { hasAnswered });
    }
}