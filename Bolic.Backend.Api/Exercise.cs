namespace Bolic.Backend.Api;

public record Exercise
{
    [JsonProperty("id")] public required string Id { get; init; } = Guid.NewGuid().ToString();
    public required string UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string TargetPosition { get; init; } = string.Empty; // lengthened, short etc.
    public string Category { get; init; } = string.Empty;
    public string Equipment { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public List<TrainingSet> Sets { get; init; } = [];
}