namespace api.Interfaces;

public interface IUserRepository
{
    public Task<UpdateResult> UpdateByIdAsync(string userId, ArchitectureUpdateDto userInput, CancellationToken cancellationToken);
}
