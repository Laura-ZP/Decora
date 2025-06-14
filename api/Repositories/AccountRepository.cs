namespace api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IMongoCollection<AppUser> _collection;

    // Dependency Injection
    public AccountRepository(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
    }

    public async Task<LoggedInDto?> RegisterAsync(AppUser userIn, CancellationToken cancellationToken)
    {
        AppUser user = await _collection.Find(doc =>
            doc.Email == userIn.Email).FirstOrDefaultAsync(cancellationToken);

        if (user is not null)
            return null;

        await _collection.InsertOneAsync(userIn, null, cancellationToken);

        LoggedInDto loggedInDto = new(
            Email: userIn.Email,
            Name: userIn.Name
        );

        return loggedInDto;
    }

    public async Task<LoggedInDto?> LoginAsync(LoginDto userIn, CancellationToken cancellationToken)
    {
        AppUser User = await _collection.Find(doc => doc.Email == userIn.Email && doc.Password == userIn.Password)
        .FirstOrDefaultAsync(cancellationToken);

        if (User is null)
            return null;

        LoggedInDto loggedInDto = new(
            Email: User.Email,
            Name: User.Name
        );

        return loggedInDto;
    }

    public async Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find<AppUser>(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return await _collection.DeleteOneAsync<AppUser>(doc => doc.Id == userId, cancellationToken);
    }
}
