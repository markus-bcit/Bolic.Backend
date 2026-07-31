namespace Bolic.Backend.Core.Transformers;

public static class MacrocycleTransformer
{
    public static Eff<Domain.Macrocycle> ToDt(this Api.Macrocycle m, string userId) =>
        liftEff(_ => new Domain.Macrocycle(
            Id: parseGuid(m.id ?? ""),
            UserId: parseGuid(userId),
            MesocycleId: parseGuid(m.mesocycleId ?? ""),
            Name: m.name,
            Description: m.description,
            StartDate: m.startDate ?? Option<DateTime>.None,
            EndDate: m.endDate ?? Option<DateTime>.None,
            Version: m.version,
            Microcycles: m.microcycles.Select(microcycle => microcycle.ToDt(userId))
                .Select(a => a.Run().ThrowIfFail()).ToList()
        ));

    public static Eff<Api.Macrocycle> ToApi(this Domain.Macrocycle m, string userId) =>
        liftEff(_ => new Api.Macrocycle()
        {
            id = m.Id.Match(id => id.ToString(), () => throw new Exceptional("Missing Id", 0015)),
            mesocycleId = m.MesocycleId.Match(id => id.ToString(), () => ""),
            name = m.Name.IfNone(""),
            description = m.Description.IfNone(""),
            startDate = m.StartDate.IfNone(DateTime.MinValue),
            endDate = m.EndDate.IfNone(DateTime.MinValue),
            version = m.Version.IfNone(0),
            microcycles = m.Microcycles.Select(MicrocycleTransformer.ToApi)
                .Select(a => a.Run().ThrowIfFail()).ToList()
        });
}