using Microsoft.OpenApi.Models;
using System.Net;

namespace Inmeta.Test.Startup.Extensions
{
    internal static class OpenApiResponsesExtensions
    {
        internal static void Add(
            this OpenApiResponses responses,
            HttpStatusCode statusCode
        )
        {
            responses.Add(
                statusCode.ToString("D"),
                new OpenApiResponse { Description = statusCode.ToString() }
            );
        }
    }
}
