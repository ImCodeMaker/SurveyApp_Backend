public interface IQuestionsStatsServices
{
    Task<QuestionsStatsResults?> getMostFrecuentAnswer(int surveyId);
}