using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Configure CORS (first)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // 2. Configure Database Context
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(60);
            }));

        // 3. Add Controllers
        builder.Services.AddControllers();

        // 4. Configure Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // 5. Register Application Services
        builder.Services.AddScoped<ISurveys, SurveysRepository>();
        builder.Services.AddScoped<ISurveyServices, SurveyServices>();
        builder.Services.AddScoped<IUser, UserRepository>();
        builder.Services.AddScoped<UsersCRUDService, UserServices>();
        builder.Services.AddScoped<IHashingServices, HashingMethod>();
        builder.Services.AddScoped<ILogin, HandleLogin>();
        builder.Services.AddScoped<IUserActions, UserActionsServices>();
        builder.Services.AddScoped<IAnswersRepository, AnswersRepository>();
        builder.Services.AddScoped<IAnswerServices, AnswerServices>();
        builder.Services.AddScoped<IQuestionsStatsServices, QuestionsStatisticsServices>();
        builder.Services.AddScoped<IQuestionsStatitisctics, QuestionsStatistics>();
        builder.Services.AddScoped<IUserFactory, UserFactory>();
        builder.Services.AddScoped<INotificationsServices, NotificacionServicio>();

        var app = builder.Build();

        // ========== MIDDLEWARE PIPELINE ========== //

        // 1. CORS (must be first)
        app.UseCors("AllowAll");

        // 2. Swagger UI (development only)
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // 3. HTTPS Redirection
        app.UseHttpsRedirection();

        // 4. Routing
        app.UseRouting();


        app.MapControllers();

        app.Run();
    }

    public class SurveyStatusBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SurveyStatusBackgroundService> _logger;
    private readonly TimeSpan _period = TimeSpan.FromHours(1); // Check every hour

    public SurveyStatusBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<SurveyStatusBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);
        
        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var surveysRepository = scope.ServiceProvider
                        .GetRequiredService<ISurveys>();
                    
                    await surveysRepository.UpdateSurveyStatusBasedOnDueDate();
                }
                
                _logger.LogInformation("Updated survey statuses based on due dates");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating survey statuses");
            }
        }
    }
}
}

