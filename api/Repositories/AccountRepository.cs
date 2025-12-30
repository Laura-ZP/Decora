using api.DTOs;

namespace api.Repositories;

public class AccountRepository : IAccountRepository
{
    #region Mongodb
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;

    // Dependency Injection
    public AccountRepository(IMongoClient client, IMyMyMongoDbSettings dbSettings, ITokenService tokenService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

        _tokenService = tokenService;
    }
    #endregion

    public async Task<LoggedInDto?> RegisterAsync(AppUser userIn, CancellationToken cancellationToken)
    {
        AppUser user = await _collection.Find(doc =>
            doc.Email == userIn.Email).FirstOrDefaultAsync(cancellationToken);

        if (user is not null)
            return null;

        await _collection.InsertOneAsync(userIn, null, cancellationToken);

        string? token =  _tokenService.CreateToken(userIn);

        return Meppers.ConvertAppUserToLoggedInDto(userIn, token);
    }

    public async Task<LoggedInDto?> LoginAsync(LoginDto userIn, CancellationToken cancellationToken)
    {
        AppUser user = await _collection.Find(doc => doc.Email == userIn.Email && doc.Password == userIn.Password)
        .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return null;

        string? token =  _tokenService.CreateToken(user);

        return Meppers.ConvertAppUserToLoggedInDto(user, token);
    }

    public async Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find<AppUser>(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return await _collection.DeleteOneAsync<AppUser>(doc => doc.Id == userId, cancellationToken);
    }
}
