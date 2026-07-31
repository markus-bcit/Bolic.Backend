namespace Bolic.Backend.Api;

public record TrainingDay
{
    public string? id { get; init; }
    public string? userId { get; init; }
    public string? microcycleId { get; init; }
    public string? trainingDayId { get; init; }
    public int? number { get; init; }
    public DateTime? createdDate { get; init; }
    public string? name { get; init; }
    public string? description { get; init; }
    public DateTime? startDate { get; init; }
    public DateTime? endDate { get; init; }
    public int version { get; init; }
    public List<TrainingExercise> exercises { get; init; } = [];
}