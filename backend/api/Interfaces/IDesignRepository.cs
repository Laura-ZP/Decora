namespace api.Interfaces;

public interface IDesignRepository
{
    public Task<Photo?> UploadDesignPhotoAsync(IFormFile file, string userId, CancellationToken cancellationToken);
}
