namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountRepository accountRepository) : ControllerBase
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

        if (loggedInDto is null)
            return BadRequest("Email or Password is wrong");

        return Ok(loggedInDto);
    }

    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<DeleteResult>> DeleteByID(string userId, CancellationToken cancellationToken)
    {
        DeleteResult? deleteResult = await accountRepository.DeleteByIdAsync(userId, cancellationToken);

        if (deleteResult is null)
            return BadRequest("Operation failed");

        return deleteResult;
    }
}
