namespace api.Interfaces;

public interface IUserRepository
{
    public Task<AppUser?> GetByIdAsync(string userId, CancellationToken cancellationToken);
    public Task<UpdateResult> UpdateByIdAsync(string userId, ArchitectUpdateDto userInput, CancellationToken cancellationToken);
    public Task<Photo?> UploadPhotoAsync(IFormFile file, string userId, string designType, CancellationToken cancellationToken);
    public Task<UpdateResult?> SetMainPhotoAsync(string userId, string photoUrlIn, CancellationToken cancellationToken);

}
