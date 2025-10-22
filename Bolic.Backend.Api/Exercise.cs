namespace Bolic.Backend.Api;

public record Exercise
{
    // ReSharper disable once InconsistentNaming
    public required string id { get; init; } = Guid.NewGuid().ToString();
    public string UserId { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string TargetPosition { get; init; } = string.Empty; // lengthened, short etc.
    public string Category { get; init; } = string.Empty;
    public string Equipment { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public List<TrainingSet> Sets { get; init; } = [];
}