using MongoDB.Driver.Linq;

namespace api.Repositories;

public class DesignRepository : IDesignRepository
{
    private readonly IMongoCollection<Design> _designCollection;
    private readonly IPhotoService _photoService;

    public DesignRepository(IMongoClient client, IMyMongoDbSettings dbSettings, IPhotoService photoService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _designCollection = dbName.GetCollection<Design>("designs");

        _photoService = photoService;
    }

    public async Task<Photo?> UploadPhotoDesignAsync(IFormFile file, string userId, CancellationToken cancellationToken)
    {
        Design? design = await _designCollection.Find(doc => doc.OwnerID == userId).SingleOrDefaultAsync(cancellationToken);

        if (design is null)
            return null;

        if (!ObjectId.TryParse(userId, out var objectId))
            return null;

        string[]? imageUrls = await _photoService.AddPhotoToDiskAsync(file, objectId);

        if (imageUrls is not null)
        {
            Photo photo;

            photo = Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: false);

            design.Photos.Add(photo);

            UpdateDefinition<Design> update = Builders<Design>.Update
                .Set(doc => doc.Photos, design.Photos);

            UpdateResult result = await _designCollection.UpdateOneAsync(doc => doc.OwnerID == userId, update, null, cancellationToken);

            return result.ModifiedCount == 1 ? photo : null;
        }

        return null;
    }

    public async Task<UpdateResult?> DeletePhotoDesignAsync(string userId, string? url_165_In, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(url_165_In)) return null;

        Photo photo = await _designCollection.AsQueryable()
            .Where(design => design.OwnerID == userId) // filter by user Id
            .SelectMany(design => design.Photos) // flatten the Photos array
            .Where(photo => photo.Url_165 == url_165_In) // filter by photo url
            .FirstOrDefaultAsync(cancellationToken); // return the photo or null

        if (photo is null) return null;

        bool isDeleteSuccess = await _photoService.DeletePhotoFromDisk(photo);

        UpdateDefinition<Design> update = Builders<Design>.Update
            .PullFilter(design => design.Photos, photo => photo.Url_165 == url_165_In);

        return await _designCollection.UpdateOneAsync<Design>(design => design.OwnerID == userId, update, null, cancellationToken);
    }
}
