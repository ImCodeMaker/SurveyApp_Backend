public interface IQuestionsStatsServices
{
    Task<MostFrequentAnswerResult?> getMostFrecuentAnswer(int surveyId, string option);
}