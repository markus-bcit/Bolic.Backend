namespace Bolic.Backend.Api;

public record TrainingDay
{
    [JsonPropertyName("id")] public required string Id { get; init; } = Guid.NewGuid().ToString();

    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public Exercise Exercise { get; init; } = new(){Id = Guid.NewGuid().ToString()};
}