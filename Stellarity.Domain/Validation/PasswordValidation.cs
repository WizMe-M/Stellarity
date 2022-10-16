using System.Text.RegularExpressions;

namespace Stellarity.Domain.Validation;

public static class PasswordValidation
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
    public const string Pattern =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[~!@#$%^&*+\-=])(?!.*\s).{8,}$";

    public static bool IsMatchPattern(in string input) => Regex.IsMatch(input, Pattern);
}