using System.Text.Json;
using UsersCRUD.Models;

namespace UsersCRUD.Data.Seeders;

public class Seed
{

    public static void SeedData(DataContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        CallEachSeeder(context, options);
    }

    public static void CallEachSeeder(DataContext context, JsonSerializerOptions options)
    {
        SeedFirstOrderTables(context, options);
        SeedSecondOrderTables(context, options);
    }

    private static void SeedFirstOrderTables(DataContext context, JsonSerializerOptions options)
    {
    }

    private static void SeedSecondOrderTables(DataContext context, JsonSerializerOptions options)
    {
        SeedUsers(context, options);
    }

    private static void SeedUsers(DataContext context, JsonSerializerOptions options)
    {
        var result = context.Users?.Any();
        if (result is true or null) return;

        var path = "Data/Seeders/user-data.json";
        var usersData = File.ReadAllText(path);
        var usersList = JsonSerializer.Deserialize<List<User>>(usersData, options) ??
                        throw new Exception("UsersData.json is empty");

        // Hash passwords
        foreach (var user in usersList)
        {
            if (string.IsNullOrWhiteSpace(user.Id))
            {
                user.Id = Guid.NewGuid().ToString();
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }

        context.Users?.AddRange(usersList);
        context.SaveChanges();
    }
}