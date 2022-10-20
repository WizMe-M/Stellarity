using Microsoft.EntityFrameworkCore;

namespace Stellarity.Database;

public static class DatabaseInitializer
{
    public static void CreateDb()
    {
        using var context = new StellarityContext();
        context.Database.Migrate();
    }
}