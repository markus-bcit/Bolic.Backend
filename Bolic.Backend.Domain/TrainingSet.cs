namespace Bolic.Backend.Models;

public record TrainingSet(
    Option<string> Id,
    Option<string> UserId,
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