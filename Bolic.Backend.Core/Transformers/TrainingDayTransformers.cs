using Bolic.Shared.Database.Api;

namespace Bolic.Backend.Core.Transformers;

public static class TrainingDayTransformers
{
    public static Option<Domain.TrainingDay> ToDt(Api.TrainingDay td) =>
        new Domain.TrainingDay(
            Id: parseGuid(td.Id),
            UserId: parseGuid(td.UserId),
            MicrocycleId: parseGuid(td.MicrocycleId),
            Name: td.Name,
            Description: td.Description,
            StartDate: td.StartDate,
            EndDate: td.EndDate,
            Exercises: td.Exercises.Select(TrainingExerciseTransformer.ToDt)
                .Select(a => a.First()).ToList()
        );

    public static Option<Api.TrainingDay> ToApi(Domain.TrainingDay td) =>
        new Api.TrainingDay()
        {
            Id = td.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
            UserId = td.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0015)),
            MicrocycleId = td.Id.Match(id => id.ToString(), () => ""),
            Name = td.Name.IfNone(string.Empty),
            Description = td.Description.IfNone(string.Empty),
            StartDate = td.StartDate.IfNone(DateTime.MinValue),
            EndDate = td.EndDate.IfNone(DateTime.MinValue),
            Exercises = td.Exercises.Select(TrainingExerciseTransformer.ToApi)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingDay", 0018))).ToList()
        };
}