public class QuestionsStatisticsServices : IQuestionsStatsServices
{
    private readonly IQuestionsStatitisctics _questionsStatistics;

    public QuestionsStatisticsServices(IQuestionsStatitisctics questionsStatistics)
    {
        _questionsStatistics = questionsStatistics;
    }

    public async Task<MostFrequentAnswerResult?> getMostFrecuentAnswer(int surveyId, string option)
    {
        return await _questionsStatistics.getMostFrecuentAnswer(surveyId,option);
    }
}