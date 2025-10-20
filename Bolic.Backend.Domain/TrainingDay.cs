namespace Bolic.Backend.Models;

public record TrainingDay(
    Option<string> Name,
    Option<string> Description,
    Option<DateTime> StartDate,
    Option<DateTime> EndDate,
    List<Exercise> Exercises
);