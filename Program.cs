using Microsoft.EntityFrameworkCore;
using UsersCRUD.Mappings;
using UsersCRUD.Data;
using UsersCRUD.Repositories;
using UsersCRUD.Repositories.Interfaces;
using UsersCRUD.Services;

var builder = WebApplication.CreateBuilder(args);

// Database connection
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Controllers
builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(typeof(UserProfile));

// Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Users CRUD API",
        Version = "v1",
        Description = "API para gestión de usuarios usando .NET 8"
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ------------------
// Database Migration + Seed
// ------------------
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        dbContext.Database.Migrate(); // Aplica migraciones
        if (app.Environment.IsDevelopment())
        {
            UsersCRUD.Data.Seeders.Seed.SeedData(dbContext);
            logger.LogInformation("✅ Database seeded successfully");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error during migration or seeding");
    }
}

// ------------------
// Middlewares
// ------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Users CRUD API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Map controllers
app.MapControllers();

app.Run();
