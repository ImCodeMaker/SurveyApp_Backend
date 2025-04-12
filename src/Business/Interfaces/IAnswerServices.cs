public interface IAnswerServices
{
    Task<List<Answer>> createAnswer(int UserId, CreateAnswerDTO createAnswerDTO);
}