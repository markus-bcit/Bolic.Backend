namespace Bolic.Backend.Core.Transformers;

public static class TrainingDayTransformer
{
    public static Option<Domain.TrainingDay> ToDt(this Api.TrainingDay td, string userId) =>
        new Domain.TrainingDay(
            Id: parseGuid(td.id ?? ""),
            UserId: parseGuid(userId),
            MicrocycleId: parseGuid(td.microcycleId ?? ""),
            TrainingDayId: parseGuid(td.trainingDayId ?? ""),
            Name: td.name,
            Description: td.description,
            StartDate: td.startDate ?? Option<DateTime>.None,
            EndDate: td.endDate ?? Option<DateTime>.None,
            Version: td.version,
            Exercises: td.exercises.Select(exercise => exercise.ToDt(userId))
                .Select(a => a.Match(ts => ts, () => throw new Exceptional("Invalid training exercise", 0043))).ToList()
        );

    public static Option<Api.TrainingDay> ToApi(this Domain.TrainingDay td) =>
        new Api.TrainingDay()
        {
            id = td.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            userId = td.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0015)),
            microcycleId = td.MicrocycleId.Match(id => id.ToString(), () => ""),
            trainingDayId =  td.TrainingDayId.Match(id => id.ToString(), () => ""),
            name = td.Name.IfNone(""),
            description = td.Description.IfNone(""),
            startDate = td.StartDate.IfNone(DateTime.MinValue),
            endDate = td.EndDate.IfNone(DateTime.MinValue),
            version = td.Version.IfNone(0),
            exercises = td.Exercises.Select(TrainingExerciseTransformer.ToApi)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingDay", 0018))).ToList()
        };
}