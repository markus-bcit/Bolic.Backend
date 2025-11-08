using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;
using Newtonsoft.Json;

namespace Bolic.Backend.Core;

public class TrainingSet(IRuntime runtime)
{
    [Function("CreateTrainingSet")]
    public async Task<HttpResponseData> CreateTrainingSet([HttpTrigger("post", Route = "sets")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingSet>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingSet>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: dt.ToApi().First(),
                    Container: "sets",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }

    [Function("PutTrainingSet")]
    public async Task<HttpResponseData> PutTrainingSet([HttpTrigger("put", Route = "sets")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingSet>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem(
                new UpdateRequest<Api.TrainingSet>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: dt.ToApi().First(),
                    Container: "sets",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("GetTrainingSet")]
    public async Task<HttpResponseData> GetTrainingSet([HttpTrigger("get", Route = "sets")] HttpRequestData req, string userId, string id)
    {
        var program =
            from request in Tap.Process<Api.TrainingSet>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.ReadItem<Api.TrainingSet>(
                new ReadRequest(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Container: "sets",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("PatchTrainingSet")]
    public async Task<HttpResponseData> PatchTrainingSet(
        [HttpTrigger("patch", Route = "sets")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingSet>(req, new() { NullValueHandling = NullValueHandling.Ignore})
            from body in request.Body.ToEff()
            from databaseResponse in CosmosDatabase.PatchItem<Api.TrainingSet>(
                new PatchRequest<Api.TrainingSet>(
                    Id: body.Id,      // take directly from the API object ToDo verify
                    UserId: body.UserId, //  ToDo verify
                    Document: body,   // pass the API object directly  ToDo verify
                    Container: "sets",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime)
            .ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }

}