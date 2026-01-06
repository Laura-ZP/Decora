using api.Extensions;

namespace api.DTOs;

public static class Mappers
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

    public static AppUser ConvertArchitectRegisterDtoToAppUser(ArchitectRegisterDto architectRegisterDto)
    {
        return new AppUser
        {
            FirstName = architectRegisterDto.FirstName,
            LastName = architectRegisterDto.LastName,
            PhoneNumber = architectRegisterDto.PhoneNumber,
            DateOfBirth = architectRegisterDto.DateOfBirth,
            LicenseNumber = architectRegisterDto.LicenseNumber,
            Providence = architectRegisterDto.Providence,
            Email = architectRegisterDto.Email,
            UserName = architectRegisterDto.Email
        };
    }

    public static AppUser ConvertClientRegisterDtoToAppUser(ClientRegisterDto clientRegisterDto)
    {
        return new AppUser
        {
            FirstName = clientRegisterDto.FirstName,
            LastName = clientRegisterDto.LastName,
            DateOfBirth = clientRegisterDto.DateOfBirth,
            PhoneNumber = clientRegisterDto.PhoneNumber,
            Email = clientRegisterDto.Email,
            UserName = clientRegisterDto.Email
        };
    }

    public static Photo ConvertPhotoUrlsToPhoto(string[] photoUrls, bool isMain)
    {
        return new Photo(
            Url_165: photoUrls[0],
            Url_256: photoUrls[1],
            Url_enlarged: photoUrls[2],
            IsMain: isMain
        );
    }

    public static MemberDto ConvertAppUserToMemberDto(AppUser appUser)
    {
        return new(
            Email: appUser.Email,
            FirstName: appUser.FirstName,
            LastName: appUser.LastName,
            Age: DateTimeExtensions.CalculateAge(appUser.DateOfBirth),
            City: appUser.City,
            Specializations: appUser.Specializations,
            YearsOfExperience: appUser.YearsOfExperience,
            Skills: appUser.Skills,
            IsAvailableForHire: appUser.IsAvailableForHire,
            RemoteWork: appUser.RemoteWork,
            Address: appUser.Address,
            LicenseVerified: appUser.LicenseVerified,
            Photos: appUser.Photos
        );
    }
}
