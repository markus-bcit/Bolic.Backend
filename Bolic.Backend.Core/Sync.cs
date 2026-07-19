using System.Reactive.Concurrency;
using Bolic.Backend.Api;
using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;
using Newtonsoft.Json.Linq;
using static Bolic.Backend.Core.Logic.TrainingSessionService;

namespace Bolic.Backend.Core;

public class Sync(Runtime runtime)
{
    [Function("sync")]
    public async Task<HttpResponseData> run([HttpTrigger(AuthorizationLevel.Anonymous, "post", "get", Route = "sync")] HttpRequestData req)
    {
        // ToDo: are we fr???
        var userId = req.Headers.GetValues("userId").FirstOrDefault() ?? "";

        var program =
            from compressedRequest in Tap.Process(req, action: Compressor.Decompress)
            from compressedBody in compressedRequest.Body
            from decompressedBody in Shared.Core.Utils.Utils.To<SyncRequest>(compressedBody)
            from syncDT in SyncRequestTransformer.ToDt(decompressedBody, userId)
            from _ in syncDT.Exercises
                .Traverse(s =>
                    from itemUserId in s.UserId.ToEff()
                    from itemId in s.Id.ToEff()
                    from item in s.ToApi()
                    from request in CosmosDatabase.UpdateItem(
                        new UpdateRequest<Api.TrainingExercise>(
                            Id: itemId.ToString(),
                            UserId: itemUserId.ToString(),
                            Document: item,
                            Container: "exercises",
                            Database: "bolic"
                        ))
                    select request
                )
            select syncDT;

        return await program.Run(runtime).ToHttpResponse(runtime, req, HttpStatusCode.Created);
    }
}