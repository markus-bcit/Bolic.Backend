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
            from dt in body.ToDt().ToEff()
            from id in dt.Id.ToEff()
            from uid in dt.UserId.ToEff()
            from api in dt.ToApi().ToEff()
            from dbr in CosmosDatabase.ReadItem<Api.TrainingDay>(
                new ReadRequest(
                    Id: id.ToString(),
                    UserId: uid.ToString(),
                    Container: "training-days",
                    Database: "bolic"
                )
            )
            from td in dbr.Match(
                Right: resp => resp.Document.ToDt().ToEff(),
                Left: ex => LanguageExt.Eff<Domain.TrainingDay>.Fail(ex)
            )
            from ts in CreateTrainingSessionFromTrainingDay(td, dt).ToEff()
            from tsId in ts.Id.ToEff()
            from tsuid in ts.UserId.ToEff()
            from tsApi in ts.ToApi().ToEff()
            from dbr2 in CosmosDatabase.CreateItem(
                new CreateRequest<Api.TrainingDay>(
                    Id: tsId.ToString(),
                    UserId: tsuid.ToString(),
                    Document: tsApi,
                    Container: "training-session",
                    Database: "bolic"
                )
            )
            select dbr2;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created, req.FunctionContext.InvocationId);
    }
    
    
    [Function("GetTrainingDay")]
    public async Task<HttpResponseData> GetTrainingSession([HttpTrigger("get", Route = "training-session")] HttpRequestData req, string userId, string id)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in body.ToDt().ToEff()
            from dtid in dt.Id.Match(a=> a, () => throw new Exceptional("Missing Id", 0034))
            from dtuid in dt.UserId.ToEff()
            from databaseResponse in CosmosDatabase.ReadItem<Api.TrainingDay>(
                new ReadRequest(
                    Id: dtid,
                    UserId: dt.UserId.First().ToString(),
                    Container: "training-sessions",
                    Database: "bolic"
                )
            )
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK, req.FunctionContext.InvocationId);
    }

}