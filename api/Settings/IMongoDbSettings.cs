namespace api.Settings;
public interface IMyMyMongoDbSettings
{
    string? ConnectionString { get; init; }
    string? DatabaseName { get; init; }
}