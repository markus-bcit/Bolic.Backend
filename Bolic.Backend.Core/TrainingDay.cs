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
        var userId = req.Headers.GetValues("userId").FirstOrDefault() ?? "";

        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body
            from dt in body.ToDt(userId)
            let udt = dt with { Id = Guid.NewGuid() }
            from id in udt.Id.ToEff()
            from uid in udt.UserId.ToEff()
            from api in udt.ToApi()
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
        
        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created);;
    }
}