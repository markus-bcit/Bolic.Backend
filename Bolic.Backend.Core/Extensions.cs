namespace Bolic.Backend;

public static class HttpResponseExtensions
{
    public static async Task<HttpResponseData> ToHttpResponse<T>(
        this Fin<Either<Exception, T>> result,
        HttpRequestData req,
        HttpStatusCode code)
    {
        return await result.Match(
            Succ: async either => await either.Match(
                Right: async r => await CreateResponse(req, code, r),
                Left: async ex => await CreateResponse(req, HttpStatusCode.BadRequest, new { error = ex.Message })
            ),
            Fail: async ex => await CreateResponse(req, HttpStatusCode.InternalServerError, new { error = ex.Message })
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