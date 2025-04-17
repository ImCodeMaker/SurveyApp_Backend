
public class CreateAnswerDTO
{
    public int Survey_Id { get; set; }
    public List<AnswersDTO>? Answers {get; set;}

    public static implicit operator CreateAnswerDTO(User v)
    {
        throw new NotImplementedException();
    }
}