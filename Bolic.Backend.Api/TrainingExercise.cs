using System.Text.Json.Serialization;

namespace Bolic.Backend.Api;

public record TrainingExercise
{
    [JsonPropertyName("id")] public string Id { get; init; } = Guid.NewGuid().ToString();

    public required string UserId { get; init; }
    public required string TrainingDayId { get; init; }
    public string Name { get; init; } = string.Empty;

    public string TargetPosition { get; init; } = string.Empty; // lengthened, short, etc.

    public string MuscleCategory { get; init; } = string.Empty;

    public string MuscleSubcategory { get; init; } = string.Empty; // optional

    public string Equipment { get; init; } = string.Empty;

    public string Notes { get; init; } = string.Empty;

    public List<TrainingSet> Sets { get; init; } = [];
}