namespace Bolic.Backend.Domain;

public record TrainingSession(

    Option<Guid> Id,
    Option<Guid> UserId,
    Option<Guid> TrainingDayId,
    Option<string> Name,
    Option<string> Description,
    Option<DateTime> StartDate,
    Option<DateTime> EndDate,
    Option<int> Version,
    List<TrainingExercise> Exercises
);