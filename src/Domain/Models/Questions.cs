public class Question
{
    public int Id { get; set; }
    public int Survey_Id { get; set; }
    public string Description { get; set; } = null!;
    public string QuestionType { get; set; } = null!;

    public Survey? Survey { get; set; }
    public ICollection<Answer>? Answers { get; set; }
}
