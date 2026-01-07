namespace api.Interfaces;

public interface IDesignRepository
{
    public Task<Photo?> UploadPhotoDesignAsync(IFormFile file, string userId, CancellationToken cancellationToken);
    public Task<UpdateResult?> DeletePhotoDesignAsync(string userId, string? url_165_In, CancellationToken cancellationToken);
}
