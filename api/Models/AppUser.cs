using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace api.Models;

[CollectionName("users")]
public class AppUser : MongoIdentityUser<ObjectId>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly DateOfBirth { get; init; }
    public string City { get; init; } = string.Empty;
    public int YearsOfExperience { get; set; }
    public List<string> Specializations { get; set; } = new();
    // ex: Residential, Commercial, Interior Design, Landscape
    public List<string> Skills { get; set; } = new();
    // ex: AutoCAD, Revit, SketchUp, BIM
    public bool IsAvailableForHire { get; set; }
    public bool RemoteWork { get; set; }
    public string Address { get; set; } = string.Empty;
    public bool LicenseVerified { get; set; }
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