using api.Controllers.Helpers;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers;

public class AccountController(IAccountRepository accountRepository) : BaseApiController
{
    [HttpPost("architecture-register")]
    public async Task<ActionResult<LoggedInDto>> ArchitectureRegister(ArchitectureRegisterDto userInput, CancellationToken cancellationToken)
    {
        if (userInput.Password != userInput.ConfirmPassword)
            return BadRequest("Your passwords do not match!");

        LoggedInDto? loggedInDto = await accountRepository.ArchitectureRegisterAsync(userInput, cancellationToken);

        if (loggedInDto?.Errors.Count() > 0)
        {
            foreach (var error in loggedInDto.Errors)
            {
                return BadRequest(error);
            }
        }

        return Ok(loggedInDto);
    }

    [HttpPost("client-register")]
    public async Task<ActionResult<LoggedInDto>> ClientRegister(ClientRegisterDto userInput, CancellationToken cancellationToken)
    {
        if (userInput.Password != userInput.ConfirmPassword)
            return BadRequest("Your passwords do not match!");

        LoggedInDto? loggedInDto = await accountRepository.ClientRegisterAsync(userInput, cancellationToken);

        if (loggedInDto?.Errors.Count() > 0)
        {
            foreach (var error in loggedInDto.Errors)
            {
                return BadRequest(error);
            }
        }

        return Ok(loggedInDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoggedInDto>> Login(LoginDto UserIn, CancellationToken cancellationToken)
    {
        LoggedInDto? loggedInDto = await accountRepository.LoginAsync(UserIn, cancellationToken);

        if (loggedInDto.IsWrongCreds)
            return BadRequest("Email or Password is wrong");

        return loggedInDto;
    }

    [Authorize]
    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<DeleteResult>> DeleteByID(string userId, CancellationToken cancellationToken)
    {
        DeleteResult? deleteResult = await accountRepository.DeleteByIdAsync(userId, cancellationToken);

        if (deleteResult is null)
            return BadRequest("Operation failed");

        return deleteResult;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<LoggedInDto>> ReloadLoggedInUser(CancellationToken cancellationToken)
    {
        // obtain token value
        string? token = null;

        bool isTokenValid = HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader);

        // Console.WriteLine(authHeader);

        if (isTokenValid)
            token = authHeader.ToString().Split(' ').Last();

        if (string.IsNullOrEmpty(token))
            return Unauthorized("Token is expired or invalid. Login again.");

        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized();

        // get loggedInDto
        LoggedInDto? loggedInDto =
        await accountRepository.ReloadLoggedInUserAsync(userId, token, cancellationToken);

        return loggedInDto is null ? Unauthorized("User is logged out or unauthorized. Login again") : loggedInDto;
    }
}
