namespace Stellarity.Domain.Authorization;

public enum AuthErrorCodes
{
    NoSuchUser = -1,
    UserNotActivated = 0,
    UserWasBanned = 1
}