using System.Text.Json.Serialization;

public class Question
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string Description { get; set; } = null!;
    public string QuestionType { get; set; } = null!;
    public ICollection<QuestionOption> Options { get; set; } = new List<QuestionOption>();

    [JsonIgnore]
    public Survey? Survey { get; set; }
    [JsonIgnore]
    public ICollection<Answer>? Answers { get; set; }
}
