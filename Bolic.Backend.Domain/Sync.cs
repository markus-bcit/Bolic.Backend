namespace Bolic.Backend.Domain;

public record Sync(
    Option<string> UserId,
    Option<DateTime> ExportDate,
    Option<string> AppVersion,
    Option<string> Platform,
    Seq<TrainingSession> TrainingSessions,
    Seq<TrainingExercise> Exercises
);