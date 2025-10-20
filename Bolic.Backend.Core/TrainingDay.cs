using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;

namespace Bolic.Backend;

public class TrainingDay(IRuntime runtime)
{
    [Function("CreateTrainingDay")]
    public async Task<HttpResponseData> CreateTrainingDay([HttpTrigger("post")] HttpRequestData req)
    {
        var program = from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dto in ConvertToDto(body).ToEff()
            from cr in FormatCreateRequest(dto, "training-days", "bolic").ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(cr)
            select databaseResponse;

        var result = program.Run((Runtime)runtime);

        return await result.ToHttpResponse(req, HttpStatusCode.Created);
    }

    public Option<Domain.TrainingDay> ConvertToDto(Api.TrainingDay trainingDay)
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

    public Option<CreateRequest<Domain.TrainingDay>> FormatCreateRequest(Domain.TrainingDay trainingDay,
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
}