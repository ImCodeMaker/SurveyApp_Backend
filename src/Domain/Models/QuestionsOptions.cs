using System.Text.Json.Serialization;

public class QuestionOption
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string OptionText { get; set; } = null!;
    
    [JsonIgnore]
    public Question? Question { get; set; }
}