namespace api.DTOs;

public record ClientRegisterDto(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string PhoneNumber,
    string Password,
    string ConfirmPassword
);

