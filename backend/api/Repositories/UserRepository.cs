using MongoDB.Driver.Linq;

namespace api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<AppUser> _collection;
    private readonly IMongoCollection<Design> _designCollection;
    private readonly ITokenService _tokenService;
    private readonly IPhotoService _photoService;

    public UserRepository(IMongoClient client, IMyMongoDbSettings dbSettings, ITokenService tokenService, IPhotoService photoService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
        _designCollection = dbName.GetCollection<Design>("designs");

        _tokenService = tokenService;
        _photoService = photoService;
    }

    public async Task<AppUser?> GetByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _collection.Find(doc => doc.Id.ToString() == userId).SingleOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return appUser;
    }

    public async Task<UpdateResult> UpdateByIdAsync(string userId, ArchitectUpdateDto userInput, CancellationToken cancellationToken)
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

    public async Task<Photo?> UploadPhotoAsync(IFormFile file, string userId, string designType, CancellationToken cancellationToken)
    {
        AppUser? appUser = await GetByIdAsync(userId, cancellationToken);

        if (appUser is null)
            return null;

        Design? design = await _designCollection
            .Find(d => d.Type == designType)
            .FirstOrDefaultAsync(cancellationToken);

        if (design is null)
            return null;

        if (!ObjectId.TryParse(userId, out var objectId))
            return null;

        string[]? imageUrls = await _photoService.AddPhotoToDiskAsync(file, objectId);

        if (imageUrls is null)
            return null;

        Photo photo = Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: false);

        // User
        appUser.Photos.Add(photo);
        await _collection.UpdateOneAsync(
            u => u.Id.ToString() == userId,
            Builders<AppUser>.Update.Set(u => u.Photos, appUser.Photos),
            null,
            cancellationToken);

        // Design
        design.Photos.Add(photo);
        await _designCollection.UpdateOneAsync(
            d => d.Type == designType,
            Builders<Design>.Update.Set(d => d.Photos, design.Photos),
            null,
            cancellationToken);

        if (design is null)
        {
            design = new Design
            {
                Type = designType,
                Photos = new List<Photo>()
            };

            // await _designCollection.InsertOneAsync(design, cancellationToken);
            await _designCollection.InsertOneAsync(design, new InsertOneOptions(), cancellationToken);
        }

        design.Photos.Add(photo);

        await _designCollection.UpdateOneAsync(
            d => d.Type == designType,
            Builders<Design>.Update.Set(d => d.Photos, design.Photos),
            null,
            cancellationToken);

        return photo;
    }

    public async Task<UpdateResult?> SetMainPhotoAsync(string userId, string photoUrlIn, CancellationToken cancellationToken)
    {
        #region  UNSET the previous main photo: Find the photo with IsMain True; update IsMain to False
        // set query
        FilterDefinition<AppUser>? filterOld = Builders<AppUser>.Filter
            .Where(appUser =>
                appUser.Id.ToString() == userId && appUser.Photos.Any<Photo>(photo => photo.IsMain == true));

        UpdateDefinition<AppUser>? updateOld = Builders<AppUser>.Update
            .Set(appUser => appUser.Photos.FirstMatchingElement().IsMain, false);

        // UpdateOneAsync(appUser => appUser.Photos.IsMain, false, null, cancellationToken);
        await _collection.UpdateOneAsync(filterOld, updateOld, null, cancellationToken);
        #endregion

        #region  SET the new main photo: find new photo by its Url_165; update IsMain to True
        FilterDefinition<AppUser>? filterNew = Builders<AppUser>.Filter
            .Where(appUser =>
                appUser.Id.ToString() == userId && appUser.Photos.Any<Photo>(photo => photo.Url_165 == photoUrlIn));

        UpdateDefinition<AppUser>? updateNew = Builders<AppUser>.Update
            .Set(appUser => appUser.Photos.FirstMatchingElement().IsMain, true);

        return await _collection.UpdateOneAsync(filterNew, updateNew, null, cancellationToken);
        #endregion
    }
}
