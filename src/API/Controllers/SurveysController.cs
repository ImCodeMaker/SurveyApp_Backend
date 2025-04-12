using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;

[ApiController]
[Route("api/[controller]")]
public class SurveysController : ControllerBase
{
    private readonly ISurveyServices _surveyServices;

    public SurveysController(ISurveyServices surveyServices)
    {
        _surveyServices = surveyServices;
    }

    [HttpPost("/survey/{UserId}")]
    public async Task<IActionResult> createTask(SurveyCreatorDTO surveyCreatorDTO,int UserId)
    {
        if (surveyCreatorDTO == null) return BadRequest();
        try
        {
            var newSurvey = await _surveyServices.createSurvey(surveyCreatorDTO,UserId);
            return Ok(newSurvey);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    [HttpGet("/surveys")]
    public IActionResult getTasks()
    {
        return Ok(_surveyServices.getAllSurveys());
    }

    [HttpGet("/survey/{Id}")]
    public async Task<IActionResult> getTasksId(int Id)
    {
        var result = await _surveyServices.getSurveyId(Id);
        return Ok(result);
    }

    [HttpPut("/survey/{Id}")]
    public async Task<IActionResult> updateSurvey(int Id,[FromBody] EditTaskDTO editTaskDTO)
    {
        if (editTaskDTO == null) return BadRequest("You must provide data to edit a task.");

        try
        {
            var newTask = new Survey{
                Id = Id,
                Title = editTaskDTO.Title,
                Description = editTaskDTO.Description,
                IsPublic = editTaskDTO.IsPublic,
                IsActive = editTaskDTO.IsActive
            };

            var savedTask = await _surveyServices.updateSurvey(newTask);
            return StatusCode(201, savedTask);
        }
        catch (System.Exception)
        {
            
            return StatusCode(500, new {message = "There was an error updating the task, try again"});
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> deleteSurvey(int Id)
    {
        return Ok(await _surveyServices.deleteSurvey(Id));
    }
}