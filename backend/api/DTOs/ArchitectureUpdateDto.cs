namespace api.DTOs;

public record ArchitectureUpdateDto(
    string City,
    int YearsOfExperience,
    List<string> Specializations,
    List<string> Skills,
    bool IsAvailableForHire,
    bool RemoteWork,
    string Address
);
