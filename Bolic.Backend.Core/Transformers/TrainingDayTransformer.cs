namespace Bolic.Backend.Core.Transformers;

public static class TrainingDayTransformer
{
    public static Option<Domain.TrainingDay> ToDt(this Api.TrainingDay td) =>
        new Domain.TrainingDay(
            Id: parseGuid(td.Id),
            UserId: parseGuid(td.UserId).IfNone(() => throw new Exceptional("Missing UserId", 0000)),
            MicrocycleId: parseGuid(td.MicrocycleId ?? string.Empty),
            TrainingDayId: parseGuid(td.TrainingDayId ?? string.Empty),
            Name: td.Name,
            Description: td.Description,
            StartDate: td.StartDate ?? Option<DateTime>.None,
            EndDate: td.EndDate ?? Option<DateTime>.None,
            Exercises: td.Exercises.Select(TrainingExerciseTransformer.ToDt)
                .Select(a => a.Match(ts => ts, () => throw new Exceptional("Invalid training exercise", 0043))).ToList()
        );

    public static Option<Api.TrainingDay> ToApi(this Domain.TrainingDay td) =>
        new Api.TrainingDay()
        {
            Id = td.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            UserId = td.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0015)),
            MicrocycleId = td.MicrocycleId.Match(id => id.ToString(), () => ""),
            TrainingDayId =  td.TrainingDayId.Match(id => id.ToString(), () => ""),
            Name = td.Name.IfNone(string.Empty),
            Description = td.Description.IfNone(string.Empty),
            StartDate = td.StartDate.IfNone(DateTime.MinValue),
            EndDate = td.EndDate.IfNone(DateTime.MinValue),
            Exercises = td.Exercises.Select(TrainingExerciseTransformer.ToApi)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingDay", 0018))).ToList()
        };
}