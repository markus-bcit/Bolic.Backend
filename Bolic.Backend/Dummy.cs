using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
using Bolic.Shared.Core;


namespace Bolic.Backend;

public class Dummy(IRuntime runtime)
{
    [Function("Dummy")]
    public HttpResponseData Run([HttpTrigger("get", "post")] HttpRequestData req)
    {
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        runtime.Logger.LogInformation("Hello World!"); 
        response.WriteStringAsync($"Worked ");
        return response;
    }

}