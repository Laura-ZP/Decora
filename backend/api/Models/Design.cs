namespace api.Models;

public class Design
{
    public string Type { get; init; } = string.Empty;
    public List<Photo> Photos { get; init; } = [];
}
