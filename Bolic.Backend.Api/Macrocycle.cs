namespace Bolic.Backend.Api;

public record Macrocycle
{
     public string? id { get; init; }
    public string? mesocycleId { get; init; }
    public string? name { get; init; }
    public string? description { get; init; }
    public DateTime? startDate { get; init; }
    public DateTime? endDate { get; init; }
    public int version { get; init; }
    public List<Microcycle> microcycles { get; init; } = [];
}