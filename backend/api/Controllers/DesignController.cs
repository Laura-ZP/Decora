using api.Controllers.Helpers;
using api.Extensions;
using api.Extensions.Validations;

namespace api.Controllers;

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

        Photo? photo = await designRepository.UploadDesignPhotoAsync(file, userId, cancellationToken);

        return photo is null ? BadRequest("Add photo failed. See logger") : photo;
    }
}
