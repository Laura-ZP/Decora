namespace api.DTOs;

public static class Meppers
{
    public static LoggedInDto ConvertAppUserToLoggedInDto(AppUser appUser, string tokenValue)
    {
        return new(
            Email: appUser.Email,
            Name: appUser.Name,
            Token: tokenValue
        );
    }
}
