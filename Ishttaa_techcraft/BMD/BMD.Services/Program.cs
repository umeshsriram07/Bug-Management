using BMD.Business.Services.Interface;
using BMD.Business.Services.Services;
using BMD.Core.Models;
using BMD.Infrastructure;
using BMD.Services.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Services.AddControllers();

    // Swagger
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    // DbContext MySQL
    builder.Services.AddDbContext<BMDDbContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            ServerVersion.AutoDetect(
                builder.Configuration.GetConnectionString("DefaultConnection")
            )
        )
    );

    // Generic Repository
    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

    // Business Services
    builder.Services.AddScoped<IBugService, BugService>();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
            policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var context =
            scope.ServiceProvider.GetRequiredService<BMDDbContext>();

        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                FullName = "Default User",
                Email = "admin@test.com",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            });

            context.SaveChanges();
        }
    }

    // Swagger
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();

        app.UseSwaggerUI();
    }

    // Global Exception Handling
    app.UseExceptionHandler("/error");

    // HTTPS
    app.UseHttpsRedirection();

    // Global Exception Handling
    app.UseMiddleware<GlobalExceptionMiddleware>();

    // CORS
    app.UseCors("AllowAll");

    // Authorization
    app.UseAuthorization();

    // Controllers
    app.MapControllers();

    // Default Route
    app.MapGet("/", () => "Bug Management API Running Successfully...");

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("Application failed to start.");
    Console.WriteLine(ex.Message);

    throw;
}