public interface IQuestionsStatitisctics
{
    Task<QuestionsStatsResults?> getQuestionStats(int surveyId);
}