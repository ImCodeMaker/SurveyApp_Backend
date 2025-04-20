using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class QuestionsStatistics : IQuestionsStatitisctics
{
    private readonly AppDbContext _db;

    public QuestionsStatistics(AppDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task<QuestionsStatsResults?> getQuestionStats(int surveyId)
    {
        try
        {
            
            var answers = await _db.Answers
                .Where(a => a.Survey_Id == surveyId)
                .Include(a => a.Question)
                .AsNoTracking() 
                .ToListAsync();

            if (!answers.Any())
            {
                return new QuestionsStatsResults
                {
                    Survey_Id = surveyId,
                    Message = "No se encontraron respuestas para esta encuesta"
                };
            }

            var allAnswers = answers
                .Select(a => a.Answer_Text?.Trim() ?? string.Empty)
                .Where(t => !string.IsNullOrEmpty(t))
                .ToList();

            var modaGlobal = allAnswers
                .GroupBy(a => a)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

           
            var results = new QuestionsStatsResults
            {
                Survey_Id = surveyId,
                Questions_Count = answers.Select(a => a.Question_Id).Distinct().Count(),
                Stats_By_Question = new Dictionary<string, QuestionStats>(),
                ModaGlobal = modaGlobal?.Key ?? "No determinada",
                ModaGlobalCount = modaGlobal?.Count() ?? 0
            };

            
            foreach (var group in answers.GroupBy(a => a.Question_Id))
            {
                var question = group.First().Question;
                var stats = new QuestionStats { Count = group.Count() };

                if (question!.QuestionType == "Scale")
                {
                    var numbers = group
                        .Where(a => int.TryParse(a.Answer_Text, out _))
                        .Select(a => int.Parse(a.Answer_Text!))
                        .ToList();

                    if (numbers.Any())
                    {
                        numbers.Sort();
                        stats.Median = numbers[numbers.Count / 2];
                        stats.Average = (int)numbers.Average();
                        stats.Mode = numbers
                            .GroupBy(n => n)
                            .OrderByDescending(g => g.Count())
                            .First()
                            .Key
                            .ToString();
                    }
                }
                else
                {
                    stats.Mode = group
                        .Select(a => a.Answer_Text?.Trim() ?? "")
                        .Where(t => !string.IsNullOrEmpty(t))
                        .GroupBy(t => t)
                        .OrderByDescending(g => g.Count())
                        .FirstOrDefault()?
                        .Key;
                }

                results.Stats_By_Question.Add(question.Description ?? $"Pregunta {question.Description}", stats);
            }

            return results;
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"Error al procesar estadísticas: {ex.Message}");

            return new QuestionsStatsResults
            {
                Survey_Id = surveyId,
                Message = "Error al calcular estadísticas",
                ErrorDetails = ex.Message
            };
        }
    }
}