namespace Bolic.Backend.Api;

public record TrainingSet
{
    public string? id { get; init; }
    public string? trainingExerciseId { get; init; }
    public string? type { get; init; } // working set, warmup etc.
    public float weight { get; init; }
    public string? weightType { get; init; }
    public float repetitions { get; init; }
    public float repetitionsInReserve { get; init; }
    public float rateOfPerceivedExertion { get; init; }
    public float quality { get; init; }
    public float averageRepetitionTime { get; init; }
    public string? notes { get; init; }
    public int version { get; init; }
}