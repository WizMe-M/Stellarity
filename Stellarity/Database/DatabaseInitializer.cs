using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database;

public static class DatabaseInitializer
{
    public static Task CreateDbAsync()
    {
        using var context = new StellarityContext();
        return context.Database.MigrateAsync();
    }
}