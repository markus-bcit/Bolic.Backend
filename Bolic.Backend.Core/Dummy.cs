using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using Bolic.Shared.Core;
using Bolic.Shared.Tap;
using LanguageExt;

namespace Bolic.Backend;

public class Dummy(IRuntime runtime)
{
    [Function("Dummy")]
    public async Task<HttpResponseData> Run([HttpTrigger("get", "post")] HttpRequestData req)
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

public class Blah
{
    public string Blahblah { get; set; } = string.Empty;
}