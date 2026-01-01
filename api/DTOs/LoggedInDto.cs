namespace api.DTOs;

public class LoggedInDto
{
    public string? Token { get; init; }
    public string? PhoneNumber { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public bool? LicenseVerified { get; init; }
    public bool? IsWrongCreds { get; set; }
    public List<string> Errors { get; set; } = [];
}
