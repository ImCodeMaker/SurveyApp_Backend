using Microsoft.EntityFrameworkCore;

public class QuestionsStatistics : IQuestionsStatitisctics
{
    private readonly AppDbContext _appDbContext;

    public QuestionsStatistics(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<MostFrequentAnswerResult?> getMostFrecuentAnswer(int surveyId, string option)
    {
        var result = await _appDbContext.Answers
            .Where(a => a.Survey_Id == surveyId && a.Question!.QuestionType == option)
            .GroupBy(a => new
            {
                a.Question_Id,
                a.Question!.Description,
                a.Answer_Text
            })
            .Select(g => new MostFrequentAnswerResult
            {
                QuestionId = g.Key.Question_Id,
                QuestionText = g.Key.Description,
                AnswerText = g.Key.Answer_Text,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync();

        return result;
    }
}
