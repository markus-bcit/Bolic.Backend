namespace Bolic.Backend.Core.Transformers;

public static class MacrocycleTransformer
{
    public static Option<Domain.Macrocycle> ToDt(this Api.Macrocycle m) =>
        new Domain.Macrocycle(
            Id: parseGuid(m.Id),
            UserId: parseGuid(m.UserId),
            MesocycleId: parseGuid(m.MesocycleId),
            Name: m.Name,
            Description: m.Description,
            StartDate: m.StartDate,
            EndDate: m.EndDate,
            Microcycles: m.Microcycles.Select(MicrocycleTransformer.ToDt)
                .Select(a => a.IfNone(() => throw new Exceptional("Invalid microcycle", 0006))).ToList()
        );

    public static Option<Api.Macrocycle> ToApi(this Domain.Macrocycle m) =>
        new Api.Macrocycle()
        {
            Id = m.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
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