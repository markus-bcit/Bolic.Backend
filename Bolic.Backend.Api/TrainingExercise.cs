using System.Text.Json.Serialization;

namespace Bolic.Backend.Api;

public record TrainingExercise
{
    [JsonPropertyName("id")] public string Id { get; init; } = Guid.NewGuid().ToString();

    public required string UserId { get; init; }
    public required string TrainingDayId { get; init; }
    public string? Name { get; init; }

    public string? TargetPosition { get; init; } // lengthened, short, etc.

    public string? MuscleCategory { get; init; }

    public string? MuscleSubcategory { get; init; } // optional

    public string? Equipment { get; init; }

    public string? Notes { get; init; }

    public List<TrainingSet> Sets { get; init; } = [];
}