using api.Controllers.Helpers;
using api.Extensions;
using api.Extensions.Validations;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Authorize]
public class DesignController(IDesignRepository designRepository) : BaseApiController
{
    [HttpPut("add-design")]
    public async Task<ActionResult<Photo>> AddDesign(
        [AllowedFileExtensions, FileSize(250_000, 4_000_000)]
        IFormFile file, CancellationToken cancellationToken
    )
    {
        if (file is null) return BadRequest("No file selected with this request");

        string? userId = User.GetUserId();

        if (userId is null) return Unauthorized("You are not logged in. please login again");

        Photo? photo = await designRepository.UploadPhotoDesignAsync(file, userId, cancellationToken);

        return photo is null ? BadRequest("Add photo failed. See logger") : photo;
    }

    [HttpPut("delete_design")]
    public async Task<ActionResult<Response>> DeleteDesign(string photoUrlIn, CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("The user is not logged in");

        UpdateResult? updateResult = await designRepository.DeletePhotoDesignAsync(userId, photoUrlIn, cancellationToken);

        return updateResult is null || !updateResult.IsModifiedCountAvailable
            ? BadRequest("Photo deletion failed. Try again in a few moments. If the issue persists contact the admin.")
            : Ok(new Response(
                Message: "Photo deleted successfully."
            ));
    }
}
