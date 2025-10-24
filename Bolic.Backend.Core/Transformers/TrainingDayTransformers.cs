using Bolic.Shared.Database.Api;

namespace Bolic.Backend.Core.Transformers;

public static class TrainingDayTransformers
{
    public static Option<Domain.TrainingDay> ToDt(Api.TrainingDay trainingDay)
    {
        return new Domain.TrainingDay(
            Id: parseGuid(trainingDay.Id),
            UserId: parseGuid(trainingDay.UserId),
            Name: trainingDay.Name,
            Description: trainingDay.Description,
            StartDate: trainingDay.StartDate,
            EndDate: trainingDay.EndDate,
            Exercises: new List<Domain.Exercise>());
    }

    public static Option<CreateRequest<Api.TrainingDay>> ToCreateRequest(Domain.TrainingDay trainingDay,
        string container, string database)
    {
        return new CreateRequest<Api.TrainingDay>(
            Id: trainingDay.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
            UserId: trainingDay.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0001)),
            Container: container,
            Database: database,
            Document: new Api.TrainingDay
            {
                Id = trainingDay.Id.Match(id => id.ToString(), () => Guid.NewGuid().ToString()),
                UserId = trainingDay.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0003)),
                Name = trainingDay.Name.IfNone(string.Empty),
                Description = trainingDay.Description.IfNone(string.Empty),
                StartDate = trainingDay.StartDate.IfNone(DateTime.UtcNow),
                EndDate = trainingDay.EndDate.IfNone(DateTime.UtcNow),
                Exercises = trainingDay.Exercises.Select(ExerciseTransformers.ToDt).ToList()
            }
        );
    }
    public static Option<UpdateRequest<Domain.TrainingDay>> ToUpdateRequest(Domain.TrainingDay trainingDay,
        string container, string database)
    {
        return new UpdateRequest<Domain.TrainingDay>(
            Id: trainingDay.Id.Match(id => id.ToString(), () => throw new Exceptional("Invalid id", 0005)),
            UserId: trainingDay.UserId.Match(id => id.ToString(), () => throw new Exceptional("Invalid UserId", 0002)),
            Container: container,
            Database: database,
            Document: trainingDay
        );
    }
    
}