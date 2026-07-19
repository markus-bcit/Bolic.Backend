namespace Bolic.Backend.Core.Transformers;

public static class MicrocycleTransformer
{
    public static Option<Domain.Microcycle> ToDt(this Api.Microcycle m, string userId) =>
        new Domain.Microcycle(
            Id: parseGuid(m.id ?? ""),
            UserId: parseGuid(userId),
            MacrocycleId: parseGuid(m.macrocycleId ?? ""),
            Name: m.name,
            Description: m.description,
            CreatedDate: m.createdDate ?? Option<DateTime>.None,
            StartDate: m.startDate ?? Option<DateTime>.None,
            EndDate: m.endDate ?? Option<DateTime>.None,
            Version: m.version,
            TrainingDays: m.trainingDays.Select(td => td.ToDt(userId))
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingDay", 0016))).ToList()
        );

    public static Option<Api.Microcycle> ToApi(this Domain.Microcycle m) =>
        new Api.Microcycle()
        {
            id = m.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            macrocycleId = m.MacrocycleId.Match(id => id.ToString(), () => ""),
            name = m.Name.IfNone(""),
            description = m.Description.IfNone(""),
            createdDate = m.CreatedDate.IfNone(DateTime.MinValue),
            startDate = m.StartDate.IfNone(DateTime.MinValue),
            endDate = m.EndDate.IfNone(DateTime.MinValue),
            version = m.Version.IfNone(0),
            trainingDays = m.TrainingDays.Select(TrainingDayTransformer.ToApi)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingDay", 0018))).ToList()
        };
}