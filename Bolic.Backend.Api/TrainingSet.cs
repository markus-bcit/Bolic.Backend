namespace Bolic.Backend.Api;

public record TrainingSet
{
    public string? id { get; init; }
    public string? userId { get; init; }
    public string? trainingExerciseId { get; init; }
    public string? type { get; init; } // working set, warmup etc.
    public float weight { get; init; }
    public string? weightType { get; init; }
    public int reps { get; init; }
    [JsonConverter(typeof(NumberToStringConverter))]
    public string? rir { get; init; }
    public int numberOfPartials { get; init; }
    public int rateOfPerceivedExertion { get; init; }
    public int quality { get; init; }
    public float averageRepetitionTime { get; init; }
    public string? notes { get; init; }
    public int version { get; init; }
    public DateTime? completedAt { get; init; }
}