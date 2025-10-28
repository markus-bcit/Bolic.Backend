using Bolic.Backend.Api;

namespace Bolic.Backend.Domain;

public record Macrocycle
(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<Guid> MesocycleId,
    Option<string> Name,
    Option<string> Description ,
    Option<DateTime> StartDate ,
    Option<DateTime> EndDate ,
    List<Microcycle> Microcycles
);