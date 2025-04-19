using System.Text.Json.Serialization;

public class Survey
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int UserId { get; set; }
    public bool IsPublic { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public DateTime Created_At { get; set; } = DateTime.Now;
    public DateTime DueDate { get; set; }

     public bool IsSurveyActive()
    {
        return DateTime.Now <= DueDate;
    }

    [JsonIgnore]
    public User? User { get; set; }
    public ICollection<Question>? Questions { get; set; }
    public ICollection<Answer>? Answers { get; set; }
}
