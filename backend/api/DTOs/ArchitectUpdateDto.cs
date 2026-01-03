namespace api.DTOs;

public record ArchitectUpdateDto(
    string City,
    int YearsOfExperience,
    List<string> Specializations,
    List<string> Skills,
    bool IsAvailableForHire,
    bool RemoteWork,
    string Address
);
