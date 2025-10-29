using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;
using Newtonsoft.Json;

namespace Bolic.Backend.Core;

public class TrainingExercise(IRuntime runtime)
{
    [Function("CreateTrainingExercise")]
    public async Task<HttpResponseData> CreateTrainingExercise([HttpTrigger("post", Route = "exercises")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingExercise>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingExercise>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: dt.ToApi().First(),
                    Container: "exercises",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }

    [Function("PutTrainingExercise")]
    public async Task<HttpResponseData> PutTrainingExercise([HttpTrigger("put", Route = "exercises")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingExercise>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem(
                new UpdateRequest<Api.TrainingExercise>(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Document: dt.ToApi().First(),
                    Container: "exercises",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("GetTrainingExercise")]
    public async Task<HttpResponseData> GetTrainingExercise([HttpTrigger("get", Route = "exercises")] HttpRequestData req, string userId, string id)
    {
        var program =
            from request in Tap.Process<Api.TrainingExercise>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.ReadItem<Api.TrainingExercise>(
                new ReadRequest(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Container: "exercises",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("PatchTrainingExercise")]
    public async Task<HttpResponseData> PatchTrainingExercise(
        [HttpTrigger("patch", Route = "exercises")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingExercise>(req, new() { NullValueHandling = NullValueHandling.Ignore})
            from body in request.Body.ToEff()
            from databaseResponse in CosmosDatabase.PatchItem<Api.TrainingExercise>(
                new PatchRequest<Api.TrainingExercise>(
                    Id: body.Id,      // take directly from the API object ToDo verify
                    UserId: body.UserId, //  ToDo verify
                    Document: body,   // pass the API object directly  ToDo verify
                    Container: "exercises",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime)
            .ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }

}