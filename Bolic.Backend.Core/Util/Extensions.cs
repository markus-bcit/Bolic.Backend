using Error = Bolic.Backend.Api.Error;

namespace Bolic.Backend.Core.Util;

public static class HttpResponseExtensions
{
    public static async Task<HttpResponseData> ToHttpResponse<T>(
        this Fin<Either<Exception, T>> result,
        Runtime rt,
        HttpRequestData req,
        HttpStatusCode code)
    {
        return await result.Match(
            Succ: async either => await either.Match(
                Right: async r => await CreateResponse(req, code, r),
                Left: async ex =>
                {
                    rt.Logger.LogWarning(ex, "Request received non-success status code, see exception for details.");
                    return ex switch
                    {
                        CosmosException cosmosEx => cosmosEx.StatusCode switch
                        {
                            HttpStatusCode.Conflict   => await CreateResponse(req, HttpStatusCode.Conflict, new Error(nameof(HttpStatusCode.Conflict), "Item already exists in database.")),
                            HttpStatusCode.NotFound   => await CreateResponse(req, HttpStatusCode.NotFound, new Error(nameof(HttpStatusCode.NotFound), "Item not found in database.")),
                            HttpStatusCode.BadRequest => await CreateResponse(req, HttpStatusCode.BadRequest, new Error(nameof(HttpStatusCode.BadRequest), "Request to database failed.")),
                            HttpStatusCode.Forbidden  => await CreateResponse(req, HttpStatusCode.Forbidden, new Error(nameof(HttpStatusCode.Forbidden),  "Request to database is forbidden.")),
                            _ => await CreateResponse(req, HttpStatusCode.InternalServerError, new { error = "Internal server error." })
                        },

                        TimeoutException => await CreateResponse(req, HttpStatusCode.RequestTimeout, new Error(nameof(HttpStatusCode.RequestTimeout),  "Request timed out.")),
                        UnauthorizedAccessException => await CreateResponse(req, HttpStatusCode.Unauthorized, new Error(nameof(HttpStatusCode.Unauthorized),  "User is unauthorized.")),
                        _ => await CreateResponse(req, HttpStatusCode.InternalServerError, new { error = "Internal server error." })
                    };
                }
            ),

            Fail: async ex =>
            {
                rt.Logger.LogError(ex, "Request received InternalServerError status code, see exception for details.");
                return await CreateResponse(req, HttpStatusCode.InternalServerError, new { error = ex.Message });
            }
        );
    }

    private static async Task<HttpResponseData> CreateResponse<T>(
        HttpRequestData req, HttpStatusCode code, T body)
    {
        var response = req.CreateResponse(code);
        await response.WriteAsJsonAsync(body);
        return response;
    }
}