namespace Bolic.Backend.Core.Logic;

public static class TrainingSessionService
{
    public static Option<Domain.TrainingDay> CreateTrainingSessionFromTrainingDay(Domain.TrainingDay trainingDay, Domain.TrainingDay trainingSession) =>
        from id in trainingSession.Id
        from trainingDayId in trainingDay.Id
        from userId in trainingSession.UserId
        from microcycleId in trainingDay.MicrocycleId
        from name in trainingDay.Name
        from description in trainingDay.Description
        select new Domain.TrainingDay(
            Id: id,
            UserId: userId,
            MicrocycleId: microcycleId,
            TrainingDayId: trainingDayId,
            Name: name,
            Description: description,
            StartDate: DateTime.UtcNow,
            EndDate: Option<DateTime>.None,
            Exercises: trainingDay.Exercises
        );
}