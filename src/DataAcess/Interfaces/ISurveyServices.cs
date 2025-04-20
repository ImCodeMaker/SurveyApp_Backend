public interface ISurveyServices
{
    Task<Survey> createSurvey(SurveyCreatorDTO survey, int Id);
    Task<Survey> deleteSurvey(int Id);
    Task<Survey> getSurveyId(int Id);
    List<Survey> getAllSurveys();
    Task<Survey> updateSurvey(Survey _survey);
}