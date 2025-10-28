
namespace Bolic.Backend.Domain;

public record TrainingExercise(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<Guid> TrainingDayId,
    Option<MuscleCategory> MuscleCategory,
    Option<MuscleSubcategory> MuscleSubcategory,
    Option<string> Name,
    Option<string> TargetPosition, // TODO: probably better on the set 
    Option<string> Equipment,
    Option<string> Notes,
    List<TrainingSet> Sets
);