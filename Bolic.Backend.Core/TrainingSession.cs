using Bolic.Backend.Api;
using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;
using Newtonsoft.Json;
using static Bolic.Backend.Core.Logic.TrainingSessionService;

namespace Bolic.Backend.Core;

/*
Idempotence: Decide if adding a set multiple times should be allowed or replaced.

Validation: Ensure the exercise exists in the session before patching.

Partial Updates: Only include fields the user modifies to save RUs.
 */
public class TrainingSession(IRuntime runtime)
{
    [Function("CreateTrainingSession")]
    public async Task<HttpResponseData> CreateTrainingSession([HttpTrigger("post", Route = "training-session")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from ts in body.ToDt().ToEff()
            from dbr in CosmosDatabase.ReadItem<Api.TrainingDay>(
                new ReadRequest(
                    Id: ts.TrainingDayId.Match(a=> a.ToString(), () => throw new Exceptional("Missing Training Id", 0033)),
                    UserId: ts.UserId.First().ToString(),
                    Container: "training-days",
                    Database: "bolic"
                )
            )
            from td in dbr.Match(
                Right: resp => resp.Document.ToDt().ToEff(),
                Left: ex => LanguageExt.Eff<Domain.TrainingDay>.Fail(ex)
            )
            from trainingSession in CreateTrainingSessionFromTrainingDay(td, ts).ToEff()
            from dbr2 in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingDay>(
                    Id: trainingSession.Id.First().ToString(),
                    UserId: trainingSession.UserId.First().ToString(),
                    Document: trainingSession.ToApi().First(),
                    Container: "training-session",
                    Database: "bolic"
                )
            )
            select dbr2;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }

    [Function("PutTrainingDay")]
    public async Task<HttpResponseData> PutTrainingSession([HttpTrigger("put", Route = "training-session")] HttpRequestData req)
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
                    Container: "training-sessions",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("GetTrainingDay")]
    public async Task<HttpResponseData> GetTrainingSession([HttpTrigger("get", Route = "training-session")] HttpRequestData req, string userId, string id)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from databaseResponse in CosmosDatabase.ReadItem<Api.TrainingDay>(
                new ReadRequest(
                    Id: dt.Id.First().ToString(),
                    UserId: dt.UserId.First().ToString(),
                    Container: "training-sessions",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }
    
    [Function("PatchTrainingDay")]
    public async Task<HttpResponseData> PatchTrainingSession(
        [HttpTrigger("patch", Route = "training-session")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req, new() { NullValueHandling = NullValueHandling.Ignore})
            from body in request.Body.ToEff()
            from databaseResponse in CosmosDatabase.PatchItem<Api.TrainingDay>(
                new PatchRequest<Api.TrainingDay>(
                    Id: body.Id,      // take directly from the API object ToDo verify
                    UserId: body.UserId, //  ToDo verify
                    Document: body,   // pass the API object directly  ToDo verify
                    Container: "training-session",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime)
            .ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }

}