using Bolic.Backend.Core.PatchOperations;
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
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingDay>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: dt.ToApi().First(),
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
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem(
                new UpdateRequest<Api.TrainingDay>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: dt.ToApi().First(),
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
            from dt in body.ToDt().ToEff()
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
            from dt in body.ToDt().ToEff()
            let po = dt.GetPatchOperations()
            from databaseResponse in CosmosDatabase.PatchItem<Api.TrainingDay>(
                new PatchRequest<Api.TrainingDay>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: dt.ToApi().First(),
                    Container: "training-days",
                    Database: "bolic",
                    Operations: po
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }

}