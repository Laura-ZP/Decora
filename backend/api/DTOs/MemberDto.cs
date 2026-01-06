namespace api.DTOs;

public record MemberDto(
    string Email,
    string FirstName,
    string LastName,
    int Age,
    string City,
    List<string> Specializations,
    int YearsOfExperience,
    List<string> Skills,
    bool IsAvailableForHire,
    bool RemoteWork,
    string Address,
    bool LicenseVerified,
    // DateTime LastActive,
    List<Photo> Photos
);