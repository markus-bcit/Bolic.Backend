namespace Bolic.Backend.Core.Transformers;

public static class MacrocycleTransformer
{
    public static Option<Domain.Macrocycle> ToDt(this Api.Macrocycle m) =>
        new Domain.Macrocycle(
            Id: parseGuid(m.Id ?? string.Empty),
            UserId: parseGuid(m.UserId).IfNone(() => throw new Exceptional("Missing UserId", 0000)),
            MesocycleId: parseGuid(m.MesocycleId ?? string.Empty),
            Name: m.Name,
            Description: m.Description,
            StartDate: m.StartDate ?? Option<DateTime>.None, 
            EndDate: m.EndDate ??  Option<DateTime>.None,
            Microcycles: m.Microcycles.Select(MicrocycleTransformer.ToDt)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid microcycle", 0006))).ToList()
        );

    public static Option<Api.Macrocycle> ToApi(this Domain.Macrocycle m) =>
        new Api.Macrocycle()
        {
            Id = m.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            UserId = m.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0004)),
            MesocycleId = m.MesocycleId.Match(id => id.ToString(), () => ""),
            Name = m.Name.IfNone(string.Empty),
            Description = m.Description.IfNone(string.Empty),
            StartDate = m.StartDate.IfNone(DateTime.MinValue),
            EndDate = m.EndDate.IfNone(DateTime.MinValue),
            Microcycles = m.Microcycles.Select(MicrocycleTransformer.ToApi)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid microcycle", 0009))).ToList()
        };
}