using Bolic.Backend.Core.PatchOperations;
using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;

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
            let udt = dt with { Id = Guid.NewGuid() }
            from id in udt.Id.ToEff()
            from uid in udt.UserId.ToEff()
            from api in udt.ToApi().ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingDay>(
                    Id: id.ToString(),
                    UserId: uid.ToString(),
                    Document: api,
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
            from id in dt.Id.ToEff(new Exceptional("Missing id", 0101))
            from uid in dt.UserId.ToEff()
            from api in dt.ToApi().ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem(
                new UpdateRequest<Api.TrainingDay>(
                    Id: id.ToString(),
                    UserId: uid.ToString(),
                    Document: api,
                    Container: "training-days",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("GetTrainingDay")]
    public async Task<HttpResponseData> GetTrainingDay([HttpTrigger("get", Route = "training-days")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from dtid in dt.Id.ToEff(new Exceptional("Missing id", 0101))
            from dtuid in dt.UserId.ToEff()
            from databaseResponse in CosmosDatabase.ReadItem<Api.TrainingDay>(
                new ReadRequest(
                    Id: dtid.ToString(),
                    UserId: dtuid.ToString(),
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
            from id in  dt.Id.ToEff(new Exceptional("Missing id", 0101))
            from uid in dt.UserId.ToEff()
            from api in dt.ToApi().ToEff()
            let po = dt.GetPatchOperations()
            from _ in guard<Error>(po.Any(), new Exceptional("No patch operations found.", 0102))
            from databaseResponse in CosmosDatabase.PatchItem<Api.TrainingDay>(
                new PatchRequest<Api.TrainingDay>(
                    Id: id.ToString(),
                    UserId: uid.ToString(),
                    Document: api,
                    Container: "training-days",
                    Database: "bolic",
                    Operations: po
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }

}