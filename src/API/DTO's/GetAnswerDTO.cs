// DTOs/UserAnswersResponse.cs
public class UserAnswersResponse
{
    public int SurveyId { get; set; }
    public string? SurveyTitle { get; set; }
    public List<QuestionAnswer> Answers { get; set; } = new();
}

public class QuestionAnswer
{
    public int QuestionId { get; set; }
    public string? QuestionText { get; set; }
    public string? QuestionType { get; set; } 
    public string? UserAnswer { get; set; }
    public DateTime AnsweredAt { get; set; }
}