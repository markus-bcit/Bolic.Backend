namespace Bolic.Backend.Api;

public record Microcycle
{
    [JsonProperty("id")] public string Id { get; init; } = Guid.NewGuid().ToString();
    public required string UserId { get; init; }
    public string MacrocycleId  { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public int Number { get; init; }
    public DateTime CreatedDate { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public List<TrainingDay> TrainingDays { get; init; } = [];
}