namespace Bolic.Backend.Api;

public record SyncResponse
(
    DateTime? syncAt,
    int exercisesCount,
    int trainingSessionsCount
);