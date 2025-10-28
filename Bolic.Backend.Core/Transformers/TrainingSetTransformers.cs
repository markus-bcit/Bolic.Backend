namespace Bolic.Backend.Core.Transformers;

public static class TrainingSetTransformers
{
    public static Option<Domain.TrainingSet> ToDt(Api.TrainingSet s) =>
        new Domain.TrainingSet(
            Id: parseGuid(s.Id),
            UserId: parseGuid(s.UserId),
            TrainingExerciseId: parseGuid(s.TrainingExerciseId),
            Type: s.Type,
            Weight: s.Weight,
            WeightType: s.WeightType,
            Repetitions: s.Repetitions,
            RepetitionsInReserve: s.RepetitionsInReserve,
            RateOfPerceivedExertion: s.RateOfPerceivedExertion,
            Quality: s.Quality,
            AverageRepetitionTime: s.AverageRepetitionTime,
            Notes: s.Notes
        );

    public static Option<Api.TrainingSet> ToApi(Domain.TrainingSet s) =>
        new Api.TrainingSet()
        {
            Id = s.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
            UserId = s.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0013)),
            TrainingExerciseId = s.TrainingExerciseId.Match(id => id.ToString(), () => ""),
            Type = s.Type.IfNone(string.Empty),
            Weight = s.Weight.IfNone(0),
            WeightType = s.WeightType.IfNone(string.Empty),
            Repetitions = s.Repetitions.IfNone(0),
            RepetitionsInReserve = s.RepetitionsInReserve.IfNone(0),
            RateOfPerceivedExertion = s.RateOfPerceivedExertion.IfNone(0),
            Quality = s.Quality.IfNone(0),
            AverageRepetitionTime = s.AverageRepetitionTime.IfNone(0),
            Notes = s.Notes.IfNone(string.Empty)
        };
}