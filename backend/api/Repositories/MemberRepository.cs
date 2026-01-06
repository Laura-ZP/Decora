using api.Helpers;

namespace api.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly IMongoCollection<AppUser> _collection;

    public MemberRepository(IMongoClient client, IMyMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
    }

    public async Task<PagedList<AppUser>> GetAllAsync(PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        IQueryable<AppUser> query = _collection.AsQueryable();

        PagedList<AppUser> appUsers = await PagedList<AppUser>.CreatePagedListAsync(
            query, paginationParams.PageNumber, paginationParams.PageSize, cancellationToken
        );

        return appUsers;
    }
}
