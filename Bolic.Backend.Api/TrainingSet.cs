namespace Bolic.Backend.Api;

public record TrainingSet
{
    [JsonProperty("id")] public string Id { get; init; } = Guid.NewGuid().ToString();
    public required string UserId { get; init; }
    public string TrainingExerciseId { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty; // working set, warmup etc.
    public float Weight { get; init; }
    public string WeightType { get; init; } = string.Empty;
    public float Repetitions { get; init; }
    public float RepetitionsInReserve { get; init; }
    public float RateOfPerceivedExertion { get; init; }
    public float Quality { get; init; }
    public float AverageRepetitionTime { get; init; }
    public string Notes { get; init; } = string.Empty;
}