using MongoDB.Driver.Linq;

namespace api.Repositories;

public class DesignRepository : IDesignRepository
{
    private readonly IMongoCollection<Design> _DesignCollection;
    private readonly IPhotoService _photoService;

    public DesignRepository(IMongoClient client, IMyMongoDbSettings dbSettings, IPhotoService photoService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _DesignCollection = dbName.GetCollection<Design>("designs");

        _photoService = photoService;
    }

    public async Task<Photo?> UploadDesignPhotoAsync(IFormFile file, string userId, CancellationToken cancellationToken)
    {
        Design? design = await _DesignCollection.Find(doc => doc.OwnerID == userId).SingleOrDefaultAsync(cancellationToken);

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

            UpdateResult result = await _DesignCollection.UpdateOneAsync(doc => doc.OwnerID == userId, update, null, cancellationToken);

            return result.ModifiedCount == 1 ? photo : null;
        }

        return null;
    }
}
