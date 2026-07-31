namespace Bolic.Backend.Domain;

public record TrainingSet(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<Guid> TrainingExerciseId,
    Option<string> Type,
    Option<float> Weight,
    Option<string> WeightType,
    Option<int> Repetitions,
    Option<string> RepetitionsInReserve,
    Option<int> NumberOfPartials,
    Option<int> RateOfPerceivedExertion,
    Option<int> Quality,
    Option<float> AverageRepetitionTime,
    Option<string> Notes,
    Option<int> Version,
    Option<DateTime> CompletedAt 
);