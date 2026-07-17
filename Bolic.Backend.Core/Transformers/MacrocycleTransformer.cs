namespace Bolic.Backend.Core.Transformers;

public static class MacrocycleTransformer
{
    public static Option<Domain.Macrocycle> ToDt(this Api.Macrocycle m) =>
        new Domain.Macrocycle(
            Id: parseGuid(m.id ?? ""),
            UserId: parseGuid(m.userId).IfNone(() => throw new Exceptional("Missing UserId", 0000)),
            MesocycleId: parseGuid(m.mesocycleId ?? ""),
            Name: m.name,
            Description: m.description,
            StartDate: m.startDate ?? Option<DateTime>.None,
            EndDate: m.endDate ??  Option<DateTime>.None,
            Version: m.version,
            Microcycles: m.microcycles.Select(MicrocycleTransformer.ToDt)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid microcycle", 0006))).ToList()
        );

    public static Option<Api.Macrocycle> ToApi(this Domain.Macrocycle m) =>
        new Api.Macrocycle()
        {
            id = m.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            userId = m.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0004)),
            mesocycleId = m.MesocycleId.Match(id => id.ToString(), () => ""),
            name = m.Name.IfNone(""),
            description = m.Description.IfNone(""),
            startDate = m.StartDate.IfNone(DateTime.MinValue),
            endDate = m.EndDate.IfNone(DateTime.MinValue),
            version = m.Version.IfNone(0),
            microcycles = m.Microcycles.Select(MicrocycleTransformer.ToApi)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid microcycle", 0009))).ToList()
        };
}