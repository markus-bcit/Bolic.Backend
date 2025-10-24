using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Implementation;
using Newtonsoft.Json;

namespace Bolic.Backend.Core;

public class TrainingDay(IRuntime runtime)
{
    [Function("CreateTrainingDay")]
    public async Task<HttpResponseData> CreateTrainingDay([HttpTrigger("post")] HttpRequestData req)
    {
        var program =
            from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in TrainingDayTransformers.ToDt(body).ToEff()
            from cr in TrainingDayTransformers.ToCreateRequest(dt, "training-days", "bolic").ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(cr)
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.Created);
    }

    [Function("UpdateTrainingDay")]
    public async Task<HttpResponseData> UpdateTrainingDay([HttpTrigger("put")] HttpRequestData req)
    {
        var program = from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dt in TrainingDayTransformers.ToDt(body).ToEff()
            from cr in TrainingDayTransformers.ToUpdateRequest(dt, "training-days", "bolic").ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem(cr)
            select databaseResponse;

        return await program.Run((Runtime)runtime).ToHttpResponse((Runtime)runtime, req, HttpStatusCode.OK);
    }
}