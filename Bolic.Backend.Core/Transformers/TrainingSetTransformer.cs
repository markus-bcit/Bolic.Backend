namespace Bolic.Backend.Core.Transformers;

public static class TrainingSetTransformer
{
    public static Eff<Domain.TrainingSet> ToDt(this Api.TrainingSet s, string userId) =>
        liftEff(_ => new Domain.TrainingSet(
            Id: parseGuid(s.id ?? ""),
            UserId: parseGuid(userId),
            TrainingExerciseId: parseGuid(s.trainingExerciseId ?? ""),
            Type: s.type,
            Weight: s.weight,
            WeightType: s.weightType,
            Repetitions: s.reps,
            RepetitionsInReserve: s.rir,
            NumberOfPartials: s.numberOfPartials,
            RateOfPerceivedExertion: s.rateOfPerceivedExertion,
            Quality: s.quality,
            AverageRepetitionTime: s.averageRepetitionTime,
            Notes: s.notes,
            Version: s.version,
            CompletedAt: s.completedAt ?? Option<DateTime>.None
        ));


    public static Eff<Api.TrainingSet> ToApi(this Domain.TrainingSet s) =>
        liftEff(_ => new Api.TrainingSet()
        {
            id = s.Id.Match(id => id.ToString(), ""),
            userId = s.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0013)),
            trainingExerciseId = s.TrainingExerciseId.Match(id => id.ToString(), () => ""),
            type = s.Type.IfNone(""),
            weight = s.Weight.IfNone(0),
            weightType = s.WeightType.IfNone(""),
            reps = s.Repetitions.IfNone(0),
            rir = s.RepetitionsInReserve.IfNone(""),
            numberOfPartials = s.NumberOfPartials.IfNone(0),
            rateOfPerceivedExertion = s.RateOfPerceivedExertion.IfNone(0),
            quality = s.Quality.IfNone(0),
            averageRepetitionTime = s.AverageRepetitionTime.IfNone(0),
            notes = s.Notes.IfNone(""),
            version = s.Version.IfNone(0),
            completedAt = s.CompletedAt.IfNone(DateTime.MinValue),
        });
}