using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AnswersController : ControllerBase
{
    private readonly IAnswerServices _answerServices;
    private readonly AppDbContext _appDbContext;
    private readonly UsersCRUDService _usersCRUDService;
    private readonly INotificationsServices _notificationsServices;

    // Make sure this is the ONLY constructor in your controller
    public AnswersController(IAnswerServices answerServices, AppDbContext appDbContext, UsersCRUDService usersCRUDService, INotificationsServices notificationsServices)
    {
        _answerServices = answerServices;
        _appDbContext = appDbContext;
        _usersCRUDService = usersCRUDService;
        _notificationsServices = notificationsServices;
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
            var findUser = await _usersCRUDService.GetUsersID(userId);

            var asunto = "Confirmación de respuestas";
            var mensaje = $"Hola {findUser.Name},\n\nGracias por completar nuestra encuesta. Hemos recibido tus respuestas correctamente.";

            await _notificationsServices.EnviarNotificacionAsync(
                findUser.Email,
                asunto,
                mensaje);

            var AdminMessage = $"Hola,\n\n{findUser.Name} ha finalizado una encuesta. Puedes revisar los resultados en el panel de administración.";
            await _notificationsServices.SendAdminEmail(AdminMessage);


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

    [HttpGet("user-responses/{surveyId}/{userId}")]
    public async Task<IActionResult> GetUserResponses(int surveyId, int userId)
    {
        var survey = await _appDbContext.Surveys
            .FirstOrDefaultAsync(s => s.Id == surveyId);

        if (survey == null)
            return NotFound("Encuesta no encontrada");

        var answers = await _appDbContext.Answers
            .Where(a => a.Survey_Id == surveyId && a.User_Id == userId)
            .Include(a => a.Question)
            .OrderBy(a => a.Question!.Id)
            .ToListAsync();

        var response = new UserAnswersResponse
        {
            SurveyId = survey.Id,
            SurveyTitle = survey.Title,
            Answers = answers.Select(a => new QuestionAnswer
            {
                QuestionId = a.Question_Id,
                QuestionText = a.Question!.Description,
                QuestionType = a.Question.QuestionType,
                UserAnswer = a.Answer_Text ?? "Sin respuesta",
                AnsweredAt = a.Created_At
            }).ToList()
        };

        return Ok(response);
    }
}