namespace Bolic.Backend.Core.Transformers;

public static class TrainingSetTransformer
{
    public static Option<Domain.TrainingSet> ToDt(this Api.TrainingSet s, string userId) =>
        new Domain.TrainingSet(
            Id: parseGuid(s.id ?? ""),
            UserId: parseGuid(userId),
            TrainingExerciseId: parseGuid(s.trainingExerciseId ?? ""),
            Type: s.type,
            Weight: s.weight,
            WeightType: s.weightType,
            Repetitions: s.repetitions,
            RepetitionsInReserve: s.repetitionsInReserve,
            RateOfPerceivedExertion: s.rateOfPerceivedExertion,
            Quality: s.quality,
            AverageRepetitionTime: s.averageRepetitionTime,
            Notes: s.notes,
            Version: s.version
        );

    public static Option<Api.TrainingSet> ToApi(this Domain.TrainingSet s) =>
        new Api.TrainingSet()
        {
            id = s.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            trainingExerciseId = s.TrainingExerciseId.Match(id => id.ToString(), () => ""),
            type = s.Type.IfNone(""),
            weight = s.Weight.IfNone(0),
            weightType = s.WeightType.IfNone(""),
            repetitions = s.Repetitions.IfNone(0),
            repetitionsInReserve = s.RepetitionsInReserve.IfNone(0),
            rateOfPerceivedExertion = s.RateOfPerceivedExertion.IfNone(0),
            quality = s.Quality.IfNone(0),
            averageRepetitionTime = s.AverageRepetitionTime.IfNone(0),
            notes = s.Notes.IfNone(""),
            version = s.Version.IfNone(0)
        };
}