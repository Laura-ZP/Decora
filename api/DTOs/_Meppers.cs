using api.Extensions;

namespace api.DTOs;

public static class Meppers
{
    public static LoggedInDto ConvertAppUserToLoggedInDto(AppUser appUser, string tokenValue)
    {
        return new LoggedInDto
        {
            Token = tokenValue,
            PhoneNumber = appUser.PhoneNumber,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            LicenseVerified = appUser.LicenseVerified
        };
    }

    public static AppUser ConvertArchitectureRegisterDtoToAppUser(ArchitectureRegisterDto architectureRegisterDto)
    {
        return new AppUser
        {
            FirstName = architectureRegisterDto.FirstName,
            LastName = architectureRegisterDto.LastName,
            PhoneNumber = architectureRegisterDto.PhoneNumber,
            DateOfBirth = architectureRegisterDto.DateOfBirth,
            LicenseNumber = architectureRegisterDto.licenseNumber,
            Providence = architectureRegisterDto.Providence
        };
    }

    public static AppUser ConvertClientRegisterDtoToAppUser(ClientRegisterDto clientRegisterDto)
    {
        return new AppUser
        {
            FirstName = clientRegisterDto.FirstName,
            LastName = clientRegisterDto.LastName,
            DateOfBirth = clientRegisterDto.DateOfBirth,
            PhoneNumber = clientRegisterDto.PhoneNumber
        };
    }
}
