namespace api.DTOs;

public record ArchitectureRegisterDto(
    string FirstName,
    string LastName,
    string PhoneNumber,
    DateOnly DateOfBirth,
    string Password,
    string ConfirmPassword,
    string NationalCode,
    string licenseNumber,
    string Providence,
    [EmailAddress]
    string Email
);
