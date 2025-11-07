namespace Bolic.Backend.Api;

public record Macrocycle
{
    [JsonProperty("id")] public string Id { get; init; }
    public required string UserId { get; init; }
    public string? MesocycleId { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public List<Microcycle> Microcycles { get; init; } = [];
}