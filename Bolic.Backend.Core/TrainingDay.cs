using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;
using Newtonsoft.Json;

namespace Bolic.Backend.Core;

public class TrainingDay(IRuntime runtime)
{
    [Function("CreateTrainingDay")]
    public async Task<HttpResponseData> CreateTrainingDay([HttpTrigger("post", Route = "training-days")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in TrainingDayTransformers.ToDt(body).ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingDay>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: TrainingDayTransformers.ToApi(dt).First(),
                    Container: "training-days",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }

    [Function("PutTrainingDay")]
    public async Task<HttpResponseData> PutTrainingDay([HttpTrigger("put", Route = "training-days")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in TrainingDayTransformers.ToDt(body).ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem(
                new UpdateRequest<Api.TrainingDay>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: TrainingDayTransformers.ToApi(dt).First(),
                    Container: "training-days",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("GetTrainingDay")]
    public async Task<HttpResponseData> GetTrainingDay([HttpTrigger("get", Route = "training-days")] HttpRequestData req, string userId, string id)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in TrainingDayTransformers.ToDt(body).ToEff()
            from databaseResponse in CosmosDatabase.ReadItem<Api.TrainingDay>(
                new ReadRequest(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Container: "training-days",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("PatchTrainingDay")]
    public async Task<HttpResponseData> PatchTrainingDay(
        [HttpTrigger("patch", Route = "training-days")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            // ToDo #1 start here, fix this
            from databaseResponse in CosmosDatabase.PatchItem<Api.TrainingDay>(
                new PatchRequest<Api.TrainingDay>(
                    Id: body.Id,      // take directly from the API object ToDo verify
                    UserId: body.UserId, //  ToDo verify
                    Document: body,   // pass the API object directly  ToDo verify
                    Container: "training-days",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime)
            .ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }

}