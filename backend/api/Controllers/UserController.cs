using api.Controllers.Helpers;
using api.Extensions;
using api.Extensions.Validations;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Authorize]
public class UserController(IUserRepository userRepository) : BaseApiController
{
    [HttpPut("update")]
    public async Task<ActionResult<Response>> UpdateById(ArchitectUpdateDto userInput, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        if (userId is null)
            return Unauthorized("You are not logged. Please login again");

        UpdateResult result = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        return result is null || result.ModifiedCount == 0
            ? BadRequest("Update failed, Try again later.")
            : Ok(new Response(
                Message: "User has been updated successfully."
            ));
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<Photo>> AddPhoto(
    [AllowedFileExtensions, FileSize(250_000, 4_000_000)]
    IFormFile file,
    [FromForm] string designType,
    CancellationToken cancellationToken
    )
    {
        if (file is null)
            return BadRequest("No file selected with this request");

        if (string.IsNullOrEmpty(designType))
            return BadRequest("No designType selected");

        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized("You are not logged in. please login again");

        Photo? photo = await userRepository.UploadPhotoAsync(file, userId, designType, cancellationToken);

        return photo is null
            ? BadRequest("Add photo failed. See logger")
            : Ok(photo);
    }

    [HttpPut("set-main-photo")]
    public async Task<ActionResult<Response>> SetMainPhoto(string photoUrlIn, CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (userId is null)
        {
            return Unauthorized("You are not logged in. please login again");
        }

        UpdateResult? updateResult = await userRepository.SetMainPhotoAsync(userId, photoUrlIn, cancellationToken);

        return updateResult is null || !updateResult.IsModifiedCountAvailable
            ? BadRequest("Set as main photo failed. Try again in a few moments. If the issue persists contact the admin.")
            : Ok(new Response(
                Message: "Set this photo as main succeeded."
            ));
    }

    [HttpPut("delete-photo")]
    public async Task<ActionResult<Response>> DeletePhoto(string photoUrlIn, CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("The user is not logged in");

        UpdateResult? updateResult = await userRepository.DeletePhotoAsync(userId, photoUrlIn, cancellationToken);

        return updateResult is null || !updateResult.IsModifiedCountAvailable
            ? BadRequest("Photo deletion failed. Try again in a few moments. If the issue persists contact the admin.")
            : Ok(new Response(
                Message: "Photo deleted successfully."
            ));
    }

}
