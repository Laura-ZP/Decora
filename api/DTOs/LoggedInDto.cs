namespace api.DTOs;

public record LoggedInDto(
    string Email,
    string Name,
    int Age,
    string Token
);
