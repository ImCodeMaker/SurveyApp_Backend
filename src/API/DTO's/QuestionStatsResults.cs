public class QuestionsStatsResults
{
     public int Survey_Id { get; set; }
    public int Questions_Count { get; set; }
    public string? ModaGlobal { get; set; }       // Valor más frecuente
    public int ModaGlobalCount { get; set; }     // Cuántas veces apareció
    public Dictionary<string, QuestionStats>? Stats_By_Question { get; set; }
    public string? Message { get; set; }          // Mensaje de estado
    public string? ErrorDetails { get; set; } 
}