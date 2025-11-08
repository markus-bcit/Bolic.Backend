namespace Bolic.Backend.Core.Logic;

public static class TrainingSessionService
{
    public static Option<Domain.TrainingDay> CreateTrainingSessionFromTrainingDay(Domain.TrainingDay day,
        Domain.TrainingDay session) =>
        new Domain.TrainingDay(
            Id: session.Id,
            UserId: session.UserId,
            MicrocycleId: day.MicrocycleId,
            TrainingDayId: day.Id,
            Name: session.Name,
            Description: session.Description,
            StartDate: DateTime.UtcNow,
            EndDate: Option<DateTime>.None,
            Exercises: day.Exercises
        );
}