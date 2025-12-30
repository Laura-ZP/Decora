using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace api.Models;

[CollectionName("users")]
public class AppUser : MongoIdentityUser<ObjectId>
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public DateOnly DateOfBirth { get; init; }
    public string City { get; init; } = string.Empty;
    public int YearsOfExperience { get; init; }
    public List<string> Specializations { get; init; } = new();
    // ex: Residential, Commercial, Interior Design, Landscape
    public List<string> Skills { get; init; } = new();
    // ex: AutoCAD, Revit, SketchUp, BIM
    public bool IsAvailableForHire { get; init; }
    public bool RemoteWork { get; init; }
    public string Address { get; init; } = string.Empty;
    public bool LicenseVerified { get; init; }
}




















// public record AppUser(
//     [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
//     string? Id,
//     [EmailAddress] string Email,
//     string Name,
//     string Surname,
//     string Password,
//     string ConfirmPassword,
//     string NationalCode,
//     DateOnly DateOfBirth,
//     string City
// );