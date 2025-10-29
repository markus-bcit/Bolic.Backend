namespace Bolic.Backend.Core.Transformers;

public static class MicrocycleTransformer
{
    public static Option<Domain.Microcycle> ToDt(this Api.Microcycle m) =>
        new Domain.Microcycle(
            Id: parseGuid(m.Id),
            UserId: parseGuid(m.UserId),
            MacrocycleId: parseGuid(m.MacrocycleId ?? string.Empty),
            Name: m.Name,
            Description: m.Description,
            CreatedDate: m.CreatedDate ?? Option<DateTime>.None,
            StartDate: m.StartDate ?? Option<DateTime>.None,
            EndDate: m.EndDate ?? Option<DateTime>.None,
            TrainingDays: m.TrainingDays.Select(TrainingDayTransformer.ToDt)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingDay", 0016))).ToList()
        );

    public static Option<Api.Microcycle> ToApi(this Domain.Microcycle m) =>
        new Api.Microcycle()
        {
            Id = m.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
            UserId = m.Id.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0001)),
            MacrocycleId = m.MacrocycleId.Match(id => id.ToString(), () => ""),
            Name = m.Name.IfNone(string.Empty),
            Description = m.Description.IfNone(string.Empty),
            CreatedDate = m.CreatedDate.IfNone(DateTime.MinValue),
            StartDate = m.StartDate.IfNone(DateTime.MinValue),
            EndDate = m.EndDate.IfNone(DateTime.MinValue),
            TrainingDays = m.TrainingDays.Select(TrainingDayTransformer.ToApi)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid TrainingDay", 0018))).ToList()
        };
}