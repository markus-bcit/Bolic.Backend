namespace Bolic.Backend.Api;

public record TrainingSet
{
    // ReSharper disable once InconsistentNaming
    public required string id { get; init; }
    public string UserId { get; init; } = string.Empty;
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