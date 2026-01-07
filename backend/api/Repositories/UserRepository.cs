using MongoDB.Driver.Linq;

namespace api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;
    private readonly IPhotoService _photoService;

    public UserRepository(IMongoClient client, IMyMongoDbSettings dbSettings, ITokenService tokenService, IPhotoService photoService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

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

    public async Task<Photo?> UploadPhotoAsync(IFormFile file, string userId, CancellationToken cancellationToken)
    {
        AppUser? appUser = await GetByIdAsync(userId, cancellationToken);

        if (appUser is null)
            return null;

        // ObjectId objectId = ObjectId.Parse(userId);

        if (!ObjectId.TryParse(userId, out var objectId))
            return null;

        string[]? imageUrls = await _photoService.AddPhotoToDiskAsync(file, objectId);

        if (imageUrls is not null)
        {
            Photo photo;

            photo = Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: false);

            appUser.Photos.Add(photo);

            UpdateDefinition<AppUser> updatedUser = Builders<AppUser>.Update
                .Set(doc => doc.Photos, appUser.Photos);

            UpdateResult result = await _collection.UpdateOneAsync(doc => doc.Id.ToString() == userId, updatedUser, null, cancellationToken);

            return result.ModifiedCount == 1 ? photo : null;
        }

        return null;
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

    public async Task<UpdateResult?> DeletePhotoAsync(string userId, string? url_165_In, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(url_165_In)) return null;

        // Find the photo in AppUser
        Photo photo = await _collection.AsQueryable()
            .Where(appUser => appUser.Id.ToString() == userId) // filter by user Id
            .SelectMany(appUser => appUser.Photos) // flatten the Photos array
            .Where(photo => photo.Url_165 == url_165_In) // filter by photo url
            .FirstOrDefaultAsync(cancellationToken); // return the photo or null

        if (photo is null) return null; // Warning: should be handled with Exception handling Middlewear to log the app's bug since it's a bug!

        if (photo.IsMain) return null; // prevent from deleting main photo!

        bool isDeleteSuccess = await _photoService.DeletePhotoFromDisk(photo);

        if (!isDeleteSuccess)
            return null;

        UpdateDefinition<AppUser> update = Builders<AppUser>.Update
            .PullFilter(appUser => appUser.Photos, photo => photo.Url_165 == url_165_In);

        return await _collection.UpdateOneAsync<AppUser>(appUser => appUser.Id.ToString() == userId, update, null, cancellationToken);
    }
}
