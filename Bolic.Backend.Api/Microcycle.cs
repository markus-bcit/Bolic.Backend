namespace Bolic.Backend.Api;

public record Microcycle
{
    [JsonProperty("id")] public string? Id { get; init; }
    public required string UserId { get; init; }
    public string? MacrocycleId  { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public int? Number { get; init; }
    public DateTime? CreatedDate { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<TrainingDay> TrainingDays { get; init; } = [];
}