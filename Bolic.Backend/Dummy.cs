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
    public HttpResponseData Run([HttpTrigger("get", "post")] HttpRequestData req)
    {
        var result = Tap.Process<Blah>(req).Run((Runtime)runtime);

        return result.Match(
            Succ: _ =>
            {
                runtime.Logger.LogInformation("Worked");
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.WriteStringAsync("Worked");
                return response;
            },
            Fail: error =>
            {
                runtime.Logger.LogError($"Effect failed: {error}");
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.WriteStringAsync("Internal server error");
                return response;
            }
        );
    }
}

public class Blah
{
    public string Blahblah { get; set; } = string.Empty;
}