using System.Data;
using Microsoft.EntityFrameworkCore;

public class SurveysRepository : ISurveys
{
    private readonly AppDbContext _appDbContext;

    public SurveysRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Survey> createSurvey(SurveyCreatorDTO survey, int Id)
    {
        try
        {
            if (survey == null) throw new NullReferenceException("You must create a Survey");

            var newSurvey = new Survey
            {
                Title = survey.Title,
                Description = survey.Description,
                UserId = Id,
                IsPublic = survey.IsPublic,
                IsActive = survey.IsActive,
                Questions = survey.Questions?.Select(question => new Question
                {
                    Description = question.Text ?? "Hello world",
                    QuestionType = question.Question_Type,
                }).ToList()
            };

            var createdSurvey = await _appDbContext!.Surveys.AddAsync(newSurvey);
            await _appDbContext.SaveChangesAsync();
            return createdSurvey.Entity;

        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<Survey> deleteSurvey(int Id)
    {
        if (Id <= 0) throw new Exception("You must provide values greater than 0");

        try
        {
            var surveytoDelete = await _appDbContext!.Surveys.FirstOrDefaultAsync(survey => survey.Id == Id);

            _appDbContext.Surveys.Remove(surveytoDelete!);
            await _appDbContext.SaveChangesAsync();
            return surveytoDelete!;

        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<Survey> getSurveyId(int Id)
    {
        if (Id <= 0) throw new Exception("This should be greater than 0");

        try
        {
            var searchForTask = await _appDbContext.Surveys
     .Include(s => s.Questions)
     .FirstOrDefaultAsync(s => s.Id == Id);
            return await Task.FromResult(searchForTask!);
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public List<Survey> getAllSurveys()
    {
        var currentTasks = _appDbContext.Surveys.ToList();

        if (!currentTasks.Any())
        {
            throw new NullReferenceException("No Surveys found");
        }

        return currentTasks;
    }

    public async Task<Survey> updateSurvey(Survey _survey)
    {
        var findTask = await _appDbContext.Surveys.FirstOrDefaultAsync(survey => survey.Id == _survey.Id);

        try
        {
            if (findTask != null)
            {
                findTask.Title = _survey.Title;
                findTask.Description = _survey.Description;
                findTask.IsPublic = _survey.IsPublic;
                findTask.IsActive = _survey.IsActive;


                var duplicates = await _appDbContext.Surveys.FirstOrDefaultAsync(duplicates => duplicates.Title == findTask.Title);

                if (duplicates != null)
                {
                    throw new DuplicateNameException("There's a task with that name, try a different one");
                }

                await _appDbContext.SaveChangesAsync();

                return await Task.FromResult(findTask);
            }
            return null!;
        }
        catch (System.Exception)
        {

            throw;
        }
    }
}