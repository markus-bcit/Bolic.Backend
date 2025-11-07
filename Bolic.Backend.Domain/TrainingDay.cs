namespace Bolic.Backend.Domain;

public record TrainingDay(
    
    Option<Guid> Id,
    Option<Guid> UserId,
    Option<Guid> MicrocycleId,
    Option<Guid> TrainingDayId,
    Option<string> Name,
    Option<string> Description,
    Option<DateTime> StartDate,
    Option<DateTime> EndDate,
    List<TrainingExercise> Exercises
);