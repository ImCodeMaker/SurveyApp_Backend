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
        var user = await _appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == Id);

        if (!user!.IsAdmin)
            throw new UnauthorizedAccessException("Only administrators can create surveys");

        if (survey == null)
            throw new NullReferenceException("You must create a Survey");

        try
        {
            var newSurvey = new Survey
            {
                Title = survey.Title,
                Description = survey.Description,
                UserId = Id,
                IsPublic = survey.IsPublic,
                IsActive = survey.DueDate >= DateTime.Now,
                DueDate = survey.DueDate,
                Questions = survey.Questions?.Select(question => new Question
                {
                    Description = question.Text ?? "Hello world",
                    QuestionType = question.Question_Type,
                    Options = ProcessQuestionOptions(question)
                }).ToList()
            };

            var createdSurvey = await _appDbContext!.Surveys.AddAsync(newSurvey);
            await _appDbContext.SaveChangesAsync();
            return createdSurvey.Entity;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error creating survey", ex);
        }
    }

    public async Task UpdateSurveyStatusBasedOnDueDate()
    {
        var surveys = await _appDbContext.Surveys.ToListAsync();

        foreach (var survey in surveys)
        {
            survey.IsActive = survey.IsSurveyActive();
        }

        await _appDbContext.SaveChangesAsync();
    }

    private List<QuestionOption> ProcessQuestionOptions(QuestionsDTO question)
    {
        var options = new List<QuestionOption>();

        switch (question.Question_Type)
        {
            case "Scale":

                if (question.Options == null || !question.Options.Any())
                {
                    options = Enumerable.Range(1, 10)
                        .Select(i => new QuestionOption { OptionText = i.ToString() })
                        .ToList();
                }
                else
                {

                    options = question.Options
                        .Select(o => new QuestionOption { OptionText = o })
                        .ToList();
                }
                break;

            case "MultipleChoice":
                if (question.Options == null || !question.Options.Any())
                    throw new ArgumentException("MultipleChoice questions must have options");

                options = question.Options
                    .Select(o => new QuestionOption { OptionText = o })
                    .ToList();
                break;

            default:
                throw new ArgumentException($"Unsupported question type: {question.Question_Type}");
        }

        return options;
    }

    public async Task<Survey> deleteSurvey(int id)
{
    if (id <= 0) throw new Exception("You must provide values greater than 0");

    try
    {
        var surveyToDelete = await _appDbContext.Surveys
            .Include(s => s.Answers)  // Make sure to load related answers
            .FirstOrDefaultAsync(s => s.Id == id);

        if (surveyToDelete == null) throw new Exception("Survey not found.");

        // Manually delete related answers
        _appDbContext.Answers.RemoveRange(surveyToDelete.Answers!);

        // Now delete the survey
        _appDbContext.Surveys.Remove(surveyToDelete);

        // Save changes to the database
        await _appDbContext.SaveChangesAsync();

        return surveyToDelete;
    }
    catch (Exception ex)
    {
        // Log the exception (optional)
        throw new Exception("Error deleting survey.", ex);
    }
}



    public async Task<Survey> GetSurveyById(int id)
    {
        var survey = await _appDbContext.Surveys!
            .Include(s => s.Questions!)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(s => s.Id == id)
            ?? throw new Exception("Survey not found");

        if (!survey.IsSurveyActive())
        {
            throw new SurveyExpiredException(
                survey.Id,
                survey.DueDate,
                new Exception("Survey is no longer active")
            );
        }

        return survey;
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