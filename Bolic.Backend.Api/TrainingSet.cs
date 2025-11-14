namespace Bolic.Backend.Api;

public record TrainingSet
{
    [JsonProperty("id")] public string? Id { get; init; }
    public required string UserId { get; init; }
    public string? TrainingExerciseId { get; init; }
    public string? Type { get; init; } // working set, warmup etc.
    public float Weight { get; init; }
    public string? WeightType { get; init; }
    public float Repetitions { get; init; }
    public float RepetitionsInReserve { get; init; }
    public float RateOfPerceivedExertion { get; init; }
    public float Quality { get; init; }
    public float AverageRepetitionTime { get; init; }
    public string? Notes { get; init; }
}