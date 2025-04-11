using System.Threading.Tasks;

public class SurveyServices : ISurveyServices
{
    private readonly ISurveys _surveyrespository;

    public SurveyServices(ISurveys surveysRepository)
    {
        _surveyrespository = surveysRepository;
    }

    public async Task<Survey> createSurvey(SurveyCreatorDTO survey, int Id)
    {
        return await _surveyrespository!.createSurvey(survey, Id);
    }

    public async Task<Survey> deleteSurvey(int Id)
    {
        return await _surveyrespository.deleteSurvey(Id);
    }
    public async Task<Survey> getSurveyId(int Id)
    {
        return await _surveyrespository.getSurveyId(Id);
    }
    public List<Survey> getAllSurveys()
    {
        return  _surveyrespository.getAllSurveys();
    }
    public async Task<Survey> updateSurvey(Survey _survey)
    {
        return await _surveyrespository.updateSurvey(_survey);
    }
}