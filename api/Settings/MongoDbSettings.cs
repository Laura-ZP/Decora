namespace api.Settings;
public class MongoDbSettings : IMyMyMongoDbSettings
{
    public string? ConnectionString { get; init; }
    public string? DatabaseName { get; init; }
}