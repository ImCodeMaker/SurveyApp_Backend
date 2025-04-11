using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add DbContext to the DI container
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add other services to the DI container
        builder.Services.AddControllers();

        // Add Swagger for API documentation
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add your repositories and services
         builder.Services.AddScoped<ISurveys,SurveysRepository>();
        builder.Services.AddScoped<ISurveyServices,SurveyServices>();
        builder.Services.AddScoped<IUser, UserRepository>();
        builder.Services.AddScoped<UsersCRUDService, UserServices>();
        builder.Services.AddScoped<IHashingServices, HashingMethod>();
        builder.Services.AddScoped<ILogin,HandleLogin>();
        builder.Services.AddScoped<IUserActions,UserActionsServices>();
       

        // Build the application
        var app = builder.Build();

        // Configure the HTTP request pipeline (Swagger)
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enable HTTPS and routing
        app.UseHttpsRedirection();
        app.MapControllers();

        // Run the application
        app.Run();

    }
}
