using Bolic.Shared.Database.Api;

namespace Bolic.Backend.Transformers;

public static class TrainingDayTransformers
{
    public static Option<Domain.TrainingDay> ConvertToDto(Api.TrainingDay trainingDay)
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

    public static Option<CreateRequest<Domain.TrainingDay>> DtoToCreateRequest(Domain.TrainingDay trainingDay,
        Option<string> container, Option<string> database)
    {
        return new CreateRequest<Domain.TrainingDay>(
            Id: trainingDay.Id.ToString(),
            UserId: trainingDay.UserId.ToString(),
            Container: container,
            Database: database,
            Document: trainingDay
        );
    }
    public static Option<UpdateRequest<Domain.TrainingDay>> DtoToUpdateRequest(Domain.TrainingDay trainingDay,
        Option<string> container, Option<string> database)
    {
        return new UpdateRequest<Domain.TrainingDay>(
            Id: trainingDay.Id.ToString(),
            UserId: trainingDay.UserId.ToString(),
            Container: container,
            Database: database,
            Document: trainingDay
        );
    }
}