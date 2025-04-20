public interface IAnswersRepository
{
    Task<List<Answer>> createAnswer(int UserId, CreateAnswerDTO createAnswerDTO);
}