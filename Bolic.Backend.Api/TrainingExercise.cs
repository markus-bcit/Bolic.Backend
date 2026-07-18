namespace Bolic.Backend.Api;

public record TrainingExercise
{
    public string? id { get; init; }
    public List<string> trainingDayIds { get; init; } = [];
    public string? name { get; init; }
    public string? targetRepetitions { get; init; }
    public string? targetRepetitionsInReserve { get; init; }
    public int targetNumberOfSets { get; init; }
    public string? targetPosition { get; init; } // lengthened, short, etc.

    public string? muscleCategory { get; init; }

    public string? muscleSubcategory { get; init; } // optional

    public string? equipment { get; init; }

    public string? notes { get; init; }

    public int version { get; init; }

    public List<TrainingSet> sets { get; init; } = [];
}