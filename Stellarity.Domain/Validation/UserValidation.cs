using System.Net.Mail;
using System.Text.RegularExpressions;
using Stellarity.Database.Entities;

namespace Stellarity.Domain.Validation;

public class UserValidation
{
    /// <summary>
    /// <list type="bullet">
    /// <listheader>Represents pattern, that matching password (string) with:</listheader>
    /// <item>At least one lower case char</item>
    /// <item>At least one upper case char</item>
    /// <item>At least one digit</item>
    /// <item>At least one special symbol</item>
    /// <item>No spaces</item>
    /// <item>Length at least 8 characters</item> 
    /// </list> 
    /// </summary>
    public const string PasswordPattern =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[~!@#$%^&*+\-=])(?!.*\s).{8,}$";

    public static bool IsRealEmail(in string input) => MailAddress.TryCreate(input, out _);

    public static async Task<bool> NotExistsWithEmailAsync(string input, CancellationToken token)
        => !await AccountEntity.ExistsAsync(input, token);

    public static bool IsCorrectPassword(in string input) => Regex.IsMatch(input, PasswordPattern);
}