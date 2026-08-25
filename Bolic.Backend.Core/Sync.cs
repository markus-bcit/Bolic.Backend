using Bolic.Backend.Api;
using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;

namespace Bolic.Backend.Core;

public class Sync(Runtime runtime)
{
    private const int seconds = 5;

    [Function("sync")]
    public async Task<HttpResponseData> run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sync")] HttpRequestData req
    )
    {
        // ToDo: are we fr???
        var userId = req.Headers.GetValues("userId").FirstOrDefault() ?? "";

        var program =
            from compressedRequest in Tap.Process(req, action: Compressor.Decompress)
            from compressedBody in compressedRequest.Body
            from decompressedBody in Shared.Core.Utils.Utils.To<SyncRequest>(compressedBody)
            from syncDT in SyncRequestTransformer.ToDt(decompressedBody, userId)
            from exercisesApi in syncDT.Exercises.Traverse(s =>
                from itemUserId in s.UserId.ToEff()
                from itemId in s.Id.ToEff()
                from item in s.ToApi()
                select item
            )
            from trainingSessionApi in syncDT.TrainingSessions.Traverse(s =>
                from itemUserId in s.UserId.ToEff()
                from itemId in s.Id.ToEff()
                from item in s.ToApi()
                select item
            )
            let exerciseCount = exercisesApi.Count
            let trainingSessionCount = trainingSessionApi.Count
            from exercisesUpserts in exercisesApi.Traverse(e =>
                retry(
                    Schedule.exponential(1 * seconds) | Schedule.recurs(5),
                    from upsertResponse in CosmosDatabase.UpdateItem(
                        new UpdateRequest<Api.TrainingExercise>(
                            UserId: e.userId!, // checks in .ToApi above
                            Id: e.id!, // checks in .ToApi above
                            Document: e,
                            Container: "exercises",
                            Database: "bolic"
                        )
                    )
                    select upsertResponse
                )
            )
            from trainingSessionUpserts in trainingSessionApi.Traverse(e =>
                retry(
                    Schedule.exponential(1 * seconds) | Schedule.recurs(5),
                    from upsertResponse in CosmosDatabase.UpdateItem(
                        request: new UpdateRequest<Api.TrainingSession>(
                            UserId: e.userId!, // checks in .ToApi above
                            Id: e.id!, // checks in .ToApi above
                            Document: e,
                            Container: "training-sessions",
                            Database: "bolic"
                        )
                    )
                    select upsertResponse
                )
            )
            select new SyncResponse(DateTime.UtcNow, exerciseCount, trainingSessionCount);

        return await program.Run(runtime).ToHttpResponse(runtime, req, HttpStatusCode.OK);
    }
}
