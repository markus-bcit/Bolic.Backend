namespace Bolic.Backend.Api;

public record Exercise
{
    [JsonPropertyName("id")] public required string Id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string TargetPosition { get; init; } = string.Empty; // lengthened, short etc.
    public string Category { get; init; } = string.Empty;
    public string Equipment { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public List<Set> Sets { get; init; } = [];
}