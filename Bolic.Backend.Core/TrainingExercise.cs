using Bolic.Backend.Core.PatchOperations;
using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;

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
            let udt = dt with { Id = Guid.NewGuid() }
            from id in  udt.Id.ToEff()
            from uid in  udt.UserId.ToEff()
            from api in udt.ToApi().ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingExercise>(
                    Id: id.ToString(),
                    UserId: uid.ToString(),
                    Document: api,
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
            from id in dt.Id.ToEff(new Exceptional("Missing id", 0101))
            from uid in dt.UserId.ToEff()
            from api in dt.ToApi().ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem(
                new UpdateRequest<Api.TrainingExercise>(
                    Id: id.ToString(),
                    UserId: uid.ToString(),
                    Document: api,
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
            from pid in dt.Id.ToEff(new Exceptional("Missing id", 0101))
            from puserId in dt.UserId.ToEff()
            from api in dt.ToApi().ToEff()
            from databaseResponse in CosmosDatabase.ReadItem<Api.TrainingExercise>(
                new ReadRequest(
                    Id: pid.ToString(),
                    UserId: puserId.ToString(),
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
            from request in Tap.Process<Api.TrainingExercise>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from id in dt.Id.ToEff(new Exceptional("Missing id", 0101))
            from userId in dt.UserId.ToEff()
            from api in dt.ToApi().ToEff()
            let po = dt.GetPatchOperations()
            from _ in guard<Error>(po.Any(), new Exceptional("No patch operations found.", 0103))
            from databaseResponse in CosmosDatabase.PatchItem<Api.TrainingExercise>(
                new PatchRequest<Api.TrainingExercise>(
                    Id: id.ToString(),
                    UserId: userId.ToString(),
                    Document: api,
                    Container: "exercises",
                    Database: "bolic",
                    Operations: po
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }

}