using api.DTOs;
using Microsoft.AspNetCore.Identity;

namespace api.Repositories;

public class AccountRepository : IAccountRepository
{
    #region Mongodb
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;

    // Dependency Injection
    public AccountRepository(IMongoClient client, IMyMongoDbSettings dbSettings, ITokenService tokenService, UserManager<AppUser> userManager)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

        _tokenService = tokenService;
        _userManager = userManager;
    }
    #endregion

    public async Task<LoggedInDto?> ArchitectureRegisterAsync(ArchitectureRegisterDto userInput, CancellationToken cancellationToken)
    {
        var appUser = Mappers.ConvertArchitectureRegisterDtoToAppUser(userInput);

        var userCreationResult = await _userManager.CreateAsync(appUser, userInput.Password);

        if (!userCreationResult.Succeeded)
        {
            var errors = userCreationResult.Errors.Select(e => e.Description).ToList();

            return new LoggedInDto
            {
                Errors = errors
            };
        }

        var roleResult = await _userManager.AddToRoleAsync(appUser, "architecture");

        if (!roleResult.Succeeded)
        {
            var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();

            return new LoggedInDto
            {
                Errors = roleErrors
            };
        }

        var token = await _tokenService.CreateToken(appUser);

        if (string.IsNullOrEmpty(token))
        {
            return new LoggedInDto
            {
                Errors = new List<string> { "Failed to generate authentication token." }
            };
        }

        return Mappers.ConvertAppUserToLoggedInDto(appUser, token);
    }

    public async Task<LoggedInDto?> ClientRegisterAsync(ClientRegisterDto userInput, CancellationToken cancellationToken)
    {
        var appUser = Mappers.ConvertClientRegisterDtoToAppUser(userInput);

        var userCreationResult = await _userManager.CreateAsync(appUser, userInput.Password);

        if (!userCreationResult.Succeeded)
        {
            var errors = userCreationResult.Errors.Select(e => e.Description).ToList();

            return new LoggedInDto
            {
                Errors = errors
            };
        }

        var roleResult = await _userManager.AddToRoleAsync(appUser, "client");

        if (!roleResult.Succeeded)
        {
            var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();

            return new LoggedInDto
            {
                Errors = roleErrors
            };
        }

        var token = await _tokenService.CreateToken(appUser);

        if (string.IsNullOrEmpty(token))
        {
            return new LoggedInDto
            {
                Errors = new List<string> { "Failed to generate authentication token." }
            };
        }

        return Mappers.ConvertAppUserToLoggedInDto(appUser, token);
    }

    public async Task<LoggedInDto?> LoginAsync(LoginDto userIn, CancellationToken cancellationToken)
    {
        AppUser user = await _collection.Find(doc => doc.Email == userIn.Email && doc.Password == userIn.Password)
        .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return null;

        string? token = _tokenService.CreateToken(user);

        return Mappers.ConvertAppUserToLoggedInDto(user, token);
    }

    public async Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find<AppUser>(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return await _collection.DeleteOneAsync<AppUser>(doc => doc.Id == userId, cancellationToken);
    }
}
