using Bolic.Backend.Transformers;
using Bolic.Backend.Util;
using Bolic.Shared.Database.Implementation;

namespace Bolic.Backend;

public class TrainingDay(IRuntime runtime)
{
    [Function("CreateTrainingDay")]
    public async Task<HttpResponseData> CreateTrainingDay([HttpTrigger("post")] HttpRequestData req)
    {
        var program = from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dto in TrainingDayTransformers.ConvertToDto(body).ToEff()
            from cr in TrainingDayTransformers.DtoToCreateRequest(dto, "training-days", "bolic").ToEff()
            from databaseResponse in CosmosDatabase.CreateItem(cr)
            select databaseResponse;
        
        return await program.Run((Runtime)runtime).ToHttpResponse(req, HttpStatusCode.Created);
    }
    
    [Function("UpdateTrainingDay")]
    public async Task<HttpResponseData> UpdateTrainingDay([HttpTrigger("put")] HttpRequestData req)
    {
        var program = from request in Tap.Process<Api.TrainingDay>(req)
            from body in request.Body.ToEff()
            from dto in TrainingDayTransformers.ConvertToDto(body).ToEff()
            from cr in TrainingDayTransformers.DtoToUpdateRequest(dto, "training-days", "bolic").ToEff()
            from databaseResponse in CosmosDatabase.UpdateItem<Domain.TrainingDay>(cr)
            select databaseResponse;
        
        return await program.Run((Runtime)runtime).ToHttpResponse(req, HttpStatusCode.Created);
    }
}