using System.Text.Json.Serialization;

public class SurveyCreatorDTO
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public List<QuestionsDTO>? Questions { get; set; }
    
}