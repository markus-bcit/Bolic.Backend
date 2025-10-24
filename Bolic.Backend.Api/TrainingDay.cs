namespace Bolic.Backend.Api;

public record TrainingDay
{
    [JsonProperty("id")] public string Id { get; init; } = Guid.NewGuid().ToString();
    public required string UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public List<Exercise> Exercises { get; init; } = [];
}