using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User

        modelBuilder.Entity<User>()
    .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
        .Property(u => u.Id)
        .ValueGeneratedOnAdd();

        // Survey
        modelBuilder.Entity<Survey>()
            .HasOne(s => s.User)
            .WithMany(u => u.Surveys)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Survey)
            .WithMany(s => s.Questions)
            .HasForeignKey(q => q.SurveyId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<QuestionOption>(entity =>
       {
           entity.HasKey(qo => qo.Id);
           entity.Property(qo => qo.OptionText).IsRequired();

           // Relación con Question
           entity.HasOne(qo => qo.Question)
                 .WithMany(q => q.Options)
                 .HasForeignKey(qo => qo.QuestionId)
                 .OnDelete(DeleteBehavior.Cascade); // Si borras una pregunta, se borran sus opciones
       });

        // Answer
        modelBuilder.Entity<Answer>()
            .HasOne(a => a.User)
            .WithMany(u => u.Answers)
            .HasForeignKey(a => a.User_Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Survey)
            .WithMany(s => s.Answers)
            .HasForeignKey(a => a.Survey_Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.Question_Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}