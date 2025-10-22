namespace Bolic.Backend.Core.Transformers;

public static class ExerciseTransformers
{
    public static Api.Exercise ConvertExercise(Domain.Exercise e) =>
        new()
        {
            id = e.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
            UserId = e.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0004)),
            Name = e.Name.IfNone(string.Empty),
            TargetPosition = e.TargetPosition.IfNone(string.Empty),
            Category = e.Category.IfNone(string.Empty),
            Equipment = e.Equipment.IfNone(string.Empty),
            Notes = e.Notes.IfNone(string.Empty),
            Sets = e.Sets.Select(TrainingSetTransformers.ConvertSet).ToList()
        };
}