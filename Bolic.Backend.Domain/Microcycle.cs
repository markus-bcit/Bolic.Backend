namespace Bolic.Backend.Domain;

public record Microcycle(
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<Guid> MacrocycleId,
    Option<string> Name,
    Option<string> Description,
    Option<DateTime> CreatedDate,
    Option<DateTime> StartDate,
    Option<DateTime> EndDate,
    List<TrainingDay> TrainingDays
);