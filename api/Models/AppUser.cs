

namespace api.Models;

public record AppUser(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
    string? Id,
    [EmailAddress] string Email,
    string Name,
    string Surname,
    string Password,
    string ConfirmPassword,
    string NationalCode,
    int Age,
    string City
);