namespace Bolic.Backend.Core.Transformers;

public static class TrainingSessionTransformer
{
    public static Eff<Domain.TrainingSession> ToDt(this Api.TrainingSession td, string userId) =>
        liftEff(_ => new Domain.TrainingSession(
            Id: parseGuid(td.id).IfNone(() => throw new Exceptional("Missing id", 0040)), // Todo: figure out how to handle id relations on both c/s
            UserId: parseGuid(userId).IfNone(() => throw new Exceptional("Missing userId", 0041)),
            TrainingDayId: parseGuid(td.trainingDayId).IfNone(Guid.NewGuid), // Todo: figure out how to handle id relations on both c/s
            Name: td.name, Description: td.description, StartDate: td.startDate ?? Option<DateTime>.None,
            EndDate: td.endDate ?? Option<DateTime>.None, Version: td.version,
            Exercises: td.exercises.Select(exercise => exercise.ToDt(userId)).Select(a =>
                a.Run().ThrowIfFail()).ToList()));

    public static Eff<Api.TrainingSession> ToApi(this Domain.TrainingSession td) =>
        liftEff(_ => new Api.TrainingSession()
        {
            id = td.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            userId = td.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0015)),
            trainingDayId = td.TrainingDayId.Match(id => id.ToString(), () => ""),
            name = td.Name.IfNone(""),
            description = td.Description.IfNone(""),
            startDate = td.StartDate.IfNone(DateTime.MinValue),
            endDate = td.EndDate.IfNone(DateTime.MinValue),
            version = td.Version.IfNone(0),
            exercises = td.Exercises.Select(TrainingExerciseTransformer.ToApi)
                .Select(a => a.Run().ThrowIfFail()).ToList()
        });
}