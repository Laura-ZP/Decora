using api.Controllers.Helpers;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

[Authorize]
public class UserController(IUserRepository userRepository) : BaseApiController
{
    [HttpPut("update")]
    public async Task<ActionResult<Response>> UpdateById(ArchitectureUpdateDto userInput, CancellationToken cancellationToken)
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
}
