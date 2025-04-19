public class SurveyExpiredException : Exception
{
    public DateTime DueDate { get; }
    public int SurveyId { get; }

    // Keep your simple constructor
    public SurveyExpiredException(string message) : base(message) {}
    
    // Add richer constructors
    public SurveyExpiredException(int surveyId, DateTime dueDate) 
        : base($"Survey {surveyId} expired on {dueDate:yyyy-MM-dd}")
    {
        SurveyId = surveyId;
        DueDate = dueDate;
    }

    public SurveyExpiredException(int surveyId, DateTime dueDate, Exception innerException)
        : base($"Survey {surveyId} expired on {dueDate:yyyy-MM-dd}", innerException)
    {
        SurveyId = surveyId;
        DueDate = dueDate;
    }
}