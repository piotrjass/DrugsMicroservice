using DrugsMicroservice.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DrugsMicroservice.Application.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        // Zmieniamy DbContext na konkretny typ ApplicationDbContext
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Uruchamiamy migracje
        dbContext.Database.Migrate();
    }
}