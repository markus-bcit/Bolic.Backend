namespace Bolic.Backend;

public class TrainingDay(IRuntime runtime)
{
    [Function("CreateTrainingDay")]
    public async Task<HttpResponseData> CreateTrainingDay([HttpTrigger("get", "post")] HttpRequestData req)
    {
        var result = Tap.Process<Blah>(req).Run((Runtime)runtime);

        return await result.Match(
            Succ: async payload =>
            {
                runtime.Logger.LogInformation("Worked");
                var blah = payload.Result.Method;
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(blah);
                return response;
            },
            Fail: async error =>
            {
                runtime.Logger.LogError($"Effect failed: {error}");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("Internal server error");
                return response;
            }
        );
    }
}
