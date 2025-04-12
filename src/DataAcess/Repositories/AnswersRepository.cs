using Microsoft.EntityFrameworkCore;

public class AnswersRepository : IAnswersRepository
{
    private readonly AppDbContext _appDbContext;

    public AnswersRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<Answer>> createAnswer(int UserId, CreateAnswerDTO createAnswerDTO)
    {
        if (createAnswerDTO == null || createAnswerDTO.Answers == null)
        {
            throw new ArgumentException("Invalid input data.");
        }

        
        var surveyExists = await _appDbContext.Surveys.AnyAsync(s => s.Id == createAnswerDTO.Survey_Id);
        if (!surveyExists)
        {
            throw new InvalidOperationException($"Survey with Id {createAnswerDTO.Survey_Id} does not exist.");
        }

        
        var answers = createAnswerDTO.Answers.Select(answer => new Answer
        {
            User_Id = UserId,
            Survey_Id = createAnswerDTO.Survey_Id,
            Question_Id = answer.Question_Id,
            Answer_Text = answer.Answer_Text
        }).ToList();

        
        foreach (var answer in answers)
        {
            var questionExists = await _appDbContext.Questions.AnyAsync(q => q.Id == answer.Question_Id);
            if (!questionExists)
            {
                throw new InvalidOperationException($"Question with Id {answer.Question_Id} does not exist.");
            }
        }

        
        await _appDbContext.Answers.AddRangeAsync(answers);
        await _appDbContext.SaveChangesAsync();

        return answers;
    }
}
