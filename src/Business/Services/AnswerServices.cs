public class AnswerServices : IAnswerServices
{
    private readonly IAnswersRepository _answersRepository;

    public AnswerServices(IAnswersRepository answersRepository)
    {
        _answersRepository = answersRepository;
    }

    public async Task<List<Answer>> createAnswer(int UserId, CreateAnswerDTO createAnswerDTO)
    {
        return await _answersRepository.createAnswer(UserId, createAnswerDTO);
    }
}
