using System.Reactive.Concurrency;
using Bolic.Backend.Core.Transformers;
using Bolic.Backend.Core.Util;
using Bolic.Shared.Database.Api;
using Bolic.Shared.Database.Implementation;
using Newtonsoft.Json.Linq;
using static Bolic.Backend.Core.Logic.TrainingSessionService;

namespace Bolic.Backend.Core;

public class sync(Runtime runtime)
{
    [Function("sync")]
    public async Task<HttpResponseData> run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sync")] HttpRequestData req)
    {
        var program =
            from compressedRequest in Tap.Process(req, action: Compressor.Decompress)
            from compressedBody in compressedRequest.Body
            from decompressedBody in Shared.Core.Utils.Utils.To<JObject>(compressedBody)
            select decompressedBody;

        return await program.Run(runtime).ToHttpResponse(runtime, req, HttpStatusCode.Created);
    }
}
