using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string Password_Hash { get; set; } = null!;
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public bool IsAdmin { get; set; } = false;
    public DateTime? Created_At { get; set; } = DateTime.Now;

    
    public ICollection<Survey>? Surveys { get; set; }
    
    public ICollection<Answer>? Answers { get; set; }
}
