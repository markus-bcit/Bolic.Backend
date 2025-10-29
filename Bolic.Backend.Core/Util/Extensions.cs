using Bolic.Shared.Database.Api;
using Error = Bolic.Backend.Api.Error;

namespace Bolic.Backend.Core.Util;

public static class HttpResponseExtensions
{
    public static async Task<HttpResponseData> ToHttpResponse<T>(
        this Fin<Either<Exception, T>> result,
        Runtime rt,
        HttpRequestData req,
        HttpStatusCode code,
        string invocationId)
    {
        return await result.Match(
            Succ: async either => await either.Match(
                Right: async r => await CreateResponse(req, code, r),
                Left: async ex =>
                {
                    rt.Logger.LogError(ex,
                        "Request received non-success status code, see exception for details on {invocationId}",
                        invocationId);
                    return ex.InnerException switch
                    {
                        CosmosException cosmosEx => cosmosEx.StatusCode switch
                        {
                            HttpStatusCode.Conflict => await CreateResponse(req, HttpStatusCode.Conflict, new Error(nameof(HttpStatusCode.Conflict), "Item already exists in database.")),
                            HttpStatusCode.NotFound => await CreateResponse(req, HttpStatusCode.NotFound, new Error(nameof(HttpStatusCode.NotFound), "Item not found in database.")),
                            HttpStatusCode.BadRequest => await CreateResponse(req, HttpStatusCode.BadRequest, new Error(nameof(HttpStatusCode.BadRequest), "Request to database failed.")),
                            HttpStatusCode.Forbidden => await CreateResponse(req, HttpStatusCode.Forbidden, new Error(nameof(HttpStatusCode.Forbidden), "Request to database is forbidden.")),
                            _ => await CreateResponse(req, HttpStatusCode.InternalServerError,
                                new { error = "Internal server error." })
                        },
                        TimeoutException => await CreateResponse(req, HttpStatusCode.RequestTimeout, new Error(nameof(HttpStatusCode.RequestTimeout), "Request timed out.")),
                        UnauthorizedAccessException => await CreateResponse(req, HttpStatusCode.Unauthorized, new Error(nameof(HttpStatusCode.Unauthorized), "User is unauthorized.")),
                        _ => await CreateResponse(req, HttpStatusCode.InternalServerError, new { error = "Internal server error." })
                    };
                }
            ),
            Fail: async ex =>
            {
                rt.Logger.LogError(ex,
                    "Request received non-success status code, see exception for details on {invocationId}",
                    invocationId);
                return ex.Exception.First() switch
                {
                    // why do I need to catch both??
                    Newtonsoft.Json.JsonException => await CreateResponse(
                        req, HttpStatusCode.BadRequest,
                        new Error(nameof(HttpStatusCode.BadRequest), "Bad request payload.")
                    ),
                    _ => await CreateResponse(req, HttpStatusCode.InternalServerError, new { error = ex.Message })
                };
            }
        );
    }

    private static async Task<HttpResponseData> CreateResponse<T>(
        HttpRequestData req,
        HttpStatusCode code,
        T body)
    {
        // TODO: I don't like this, fix it
        object? responseBody = body switch
        {
            var b when b?.GetType().IsGenericType == true &&
                       b.GetType().GetGenericTypeDefinition() == typeof(CreateResponse<>) =>
                b.GetType().GetProperty("Document")?.GetValue(b),

            var b when b?.GetType().IsGenericType == true &&
                       b.GetType().GetGenericTypeDefinition() == typeof(ReadResponse<>) =>
                b.GetType().GetProperty("Document")?.GetValue(b),

            var b when b?.GetType().IsGenericType == true &&
                       b.GetType().GetGenericTypeDefinition() == typeof(UpdateResponse<>) =>
                b.GetType().GetProperty("Document")?.GetValue(b),

            var b when b?.GetType().IsGenericType == true &&
                       b.GetType().GetGenericTypeDefinition() == typeof(DeleteResponse<>) =>
                b.GetType().GetProperty("Document")?.GetValue(b),
            var b when b?.GetType().IsGenericType == true &&
                       b.GetType().GetGenericTypeDefinition() == typeof(PatchResponse<>) =>
                b.GetType().GetProperty("Document")?.GetValue(b),

            _ => body
        };

        var response = req.CreateResponse(code);
        await response.WriteAsJsonAsync(responseBody ?? "");
        return response;
    }
}