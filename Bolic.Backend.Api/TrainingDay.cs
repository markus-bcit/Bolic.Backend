namespace Bolic.Backend.Api;

public record TrainingDay
{
    [JsonProperty("id")] public string Id { get; init; } = Guid.NewGuid().ToString();
    public required string UserId { get; init; }
    public string? MicrocycleId { get; init; }
    public string? TrainingDayId { get; init; }
    public int? Number { get; init; }
    public DateTime? CreatedDate { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<TrainingExercise> Exercises { get; init; } = [];
}