using Stellarity.Database.Entities;

namespace Stellarity.Domain.Validation;

public class GameValidation
{
    public static async Task<bool> NotExistsWithTitleAsync(string title, CancellationToken token) =>
        !await GameEntity.ExistsAsync(title, token);
}