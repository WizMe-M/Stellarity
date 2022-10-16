using System.Net.Mail;
using Stellarity.Database.Entities;

namespace Stellarity.Domain.Validation;

public class EmailValidation
{
    public static bool IsRealEmail(in string input) => MailAddress.TryCreate(input, out _);

    public static async Task<bool> NotExistsAsync(string input, CancellationToken token)
        => !await AccountEntity.ExistsAsync(input, token);
}