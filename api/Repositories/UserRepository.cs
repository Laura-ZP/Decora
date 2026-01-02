namespace api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;

    public UserRepository(IMongoClient client, IMyMongoDbSettings dbSettings, ITokenService tokenService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

        _tokenService = tokenService;
    }

    public async Task<UpdateResult> UpdateByIdAsync(string userId, ArchitectureUpdateDto userInput, CancellationToken cancellationToken)
    {
        UpdateDefinition<AppUser> updateDefinition = Builders<AppUser>.Update
        .Set(appUser => appUser.City, userInput.City.Trim())
        .Set(appUser => appUser.YearsOfExperience, userInput.YearsOfExperience)
        .Set(appUser => appUser.Specializations, userInput.Specializations)
        .Set(appUser => appUser.Skills, userInput.Skills)
        .Set(appUser => appUser.IsAvailableForHire, userInput.IsAvailableForHire)
        .Set(appUser => appUser.RemoteWork, userInput.RemoteWork)
        .Set(appUser => appUser.Address, userInput.Address.Trim());

        return await _collection.UpdateOneAsync(user => user.Id.ToString() == userId, updateDefinition, null, cancellationToken);
    }
}
