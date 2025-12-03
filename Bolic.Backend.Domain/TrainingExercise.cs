namespace Bolic.Backend.Domain;

public record TrainingExercise(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<List<Guid>> TrainingDayIds,
    Option<MuscleCategory> MuscleCategory,
    Option<MuscleSubcategory> MuscleSubcategory,
    Option<string> Name,
    Option<string> TargetPosition, // TODO: probably better on the set
    Option<string> TargetRepetitions,
    Option<string> TargetRepetitionsInReserve,
    Option<int> TargetNumberOfSets,
    Option<string> Equipment,
    Option<string> Notes,
    Option<int> Version,
    List<TrainingSet> Sets
);