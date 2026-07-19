using Bolic.Shared.Database.Api;
using Error = Bolic.Backend.Api.Error;

namespace Bolic.Backend.Core.Util;

public static class HttpResponseExtensions
{
    public static async Task<HttpResponseData> ToHttpResponse<T>(
        this Fin<T> result,
        Runtime rt,
        HttpRequestData req,
        HttpStatusCode code
        )
    {
        // TODO fix at some point, looks like a🥀🥀
        return await result.Match(
            Succ: async suc =>
            {
                var res = req.CreateResponse(code);
                await res.WriteAsJsonAsync(suc);
                return res;
            },
            Fail: async err =>
            {
                rt.Logger.LogError(err, "Request received non-success status code, see exception for details on {invocationId}", req.FunctionContext.InvocationId);
                var res = req.CreateResponse(HttpStatusCode.InternalServerError);
                return res;
            });
    }

    private static async Task<HttpResponseData> CreateResponse<T>(
        HttpRequestData req,
        HttpStatusCode code,
        T body)
    {
        // TODO: I don't like this, fix it 🥀🥀
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