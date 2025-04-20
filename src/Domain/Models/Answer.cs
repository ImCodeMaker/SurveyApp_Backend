using System.Text.Json.Serialization;

public class Answer
{
    public int Id { get; set; }
    public int User_Id { get; set; }
    public int Survey_Id { get; set; }
    public int Question_Id { get; set; }
    public string? Answer_Text { get; set; }
    public DateTime Created_At { get; set; } = DateTime.Now;

    [JsonIgnore]
    public User? User { get; set; }
    [JsonIgnore]
    public Survey? Survey { get; set; }
    public Question? Question { get; set; }
}
