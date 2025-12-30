namespace api.Settings;

public class MongoDbSettings : IMyMongoDbSettings
{
    public string? ConnectionString { get; init; }
    public string? DatabaseName { get; init; }
}