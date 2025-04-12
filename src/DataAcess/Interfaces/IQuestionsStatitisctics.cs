public interface IQuestionsStatitisctics
{
    Task<MostFrequentAnswerResult?> getMostFrecuentAnswer(int surveyId, string option);
}