using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/[controller]")]
public class QuestionsStats : ControllerBase
{
    private readonly IQuestionsStatitisctics _questionStatsServices;

    public QuestionsStats(IQuestionsStatitisctics questionsStatitisctics)
    {
        _questionStatsServices = questionsStatitisctics;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMostFrequentAnswerResult(int id)
    {
        try
        {
            var result = await _questionStatsServices!.getQuestionStats(id);

            if (result == null)
            {
                return NotFound(new { message = "No se encontraron respuestas para esa encuesta y opción." });
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            // Podrías loggear el error aquí si tienes un logger inyectado
            return StatusCode(500, new { message = "Error al obtener la respuesta más frecuente.", details = ex.Message });
        }
    }

}