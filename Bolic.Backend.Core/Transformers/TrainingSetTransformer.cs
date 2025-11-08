namespace Bolic.Backend.Core.Transformers;

public static class TrainingSetTransformer
{
    public static Option<Domain.TrainingSet> ToDt(this Api.TrainingSet s) =>
        new Domain.TrainingSet(
            Id: parseGuid(s.Id ?? string.Empty),
            UserId: parseGuid(s.UserId).IfNone(() => throw new Exceptional("Missing UserId", 0000)),
            TrainingExerciseId: parseGuid(s.TrainingExerciseId ?? string.Empty),
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

    public static Option<Api.TrainingSet> ToApi(this Domain.TrainingSet s) =>
        new Api.TrainingSet()
        {
            Id = s.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
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