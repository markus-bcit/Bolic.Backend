namespace Bolic.Backend.Domain;

public record TrainingSet(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<Guid> TrainingExerciseId,
    Option<string> Type,
    Option<float> Weight,
    Option<string> WeightType,
    Option<float> Repetitions,
    Option<float> RepetitionsInReserve,
    Option<float> RateOfPerceivedExertion,
    Option<float> Quality,
    Option<float> AverageRepetitionTime,
    Option<string> Notes
);