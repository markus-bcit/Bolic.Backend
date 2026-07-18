namespace Bolic.Backend.Api;

public record Microcycle
{
    public string? id { get; init; }
    public string? macrocycleId  { get; init; }
    public string? name { get; init; }
    public string? description { get; init; }
    public int? number { get; init; }
    public DateTime? createdDate { get; init; }
    public DateTime? startDate { get; init; }
    public DateTime? endDate { get; init; }
    public int version { get; init; }
    public List<TrainingDay> trainingDays { get; init; } = [];
}