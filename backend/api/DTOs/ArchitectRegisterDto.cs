namespace api.DTOs;

public record ArchitectRegisterDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Password,
    string ConfirmPassword,
    string NationalCode,
    string LicenseNumber,
    string Providence,
    [EmailAddress]
    string Email
);
