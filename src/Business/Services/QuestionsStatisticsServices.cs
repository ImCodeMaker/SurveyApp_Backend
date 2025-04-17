public class QuestionsStatisticsServices : IQuestionsStatsServices
{
    private readonly IQuestionsStatitisctics _questionsStatistics;

    public QuestionsStatisticsServices(IQuestionsStatitisctics questionsStatistics)
    {
        _questionsStatistics = questionsStatistics;
    }

    public async Task<QuestionsStatsResults?> getMostFrecuentAnswer(int surveyId)
    {
        return await _questionsStatistics.getQuestionStats(surveyId);
    }
}