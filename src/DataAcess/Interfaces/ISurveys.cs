public interface ISurveys
{
    Task<Survey> createSurvey(SurveyCreatorDTO survey, int Id);
    Task<Survey> deleteSurvey(int Id);
    Task<Survey> GetSurveyById(int id);
    List<Survey> getAllSurveys();
    Task<Survey> updateSurvey(Survey _survey);
}