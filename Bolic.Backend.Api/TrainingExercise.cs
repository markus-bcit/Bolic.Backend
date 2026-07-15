namespace Bolic.Backend.Api;

public record TrainingExercise
{
    [JsonPropertyName("id")] public string? Id { get; init; }

    public required string UserId { get; init; }
    public List<string> TrainingDayIds { get; init; } = [];
    public string? Name { get; init; }
    public string? TargetRepetitions { get; init; }
    public string? TargetRepetitionsInReserve { get; init; }
    public int TargetNumberOfSets { get; init; }
    public string? TargetPosition { get; init; } // lengthened, short, etc.

    public string? MuscleCategory { get; init; }

    public string? MuscleSubcategory { get; init; } // optional

    public string? Equipment { get; init; }

    public string? Notes { get; init; }

    public int Version { get; init; }

    public List<TrainingSet> Sets { get; init; } = [];
}