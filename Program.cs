using Microsoft.EntityFrameworkCore;
using UserManagementApi.Mappings;
using UsersCRUD.Data;

var builder = WebApplication.CreateBuilder(args);

// Database connection
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Data Source=users.db"));

// Controllers
builder.Services.AddControllers();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Authentication & Authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

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
// Database Migration
// ------------------
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    try
    {
        // TODO: Seed initial data
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error migrating the data: {ex.Message}");
    }
}

// ------------------
// Middlewares
// ------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
