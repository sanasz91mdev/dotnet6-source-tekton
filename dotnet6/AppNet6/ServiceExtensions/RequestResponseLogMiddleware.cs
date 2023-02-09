using Serilog;
using System.Text;
using System.Text.Json;
using Cap.OpenTelemetry;
using Cap.AppMetrics;
using Prometheus;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using DataAccess.EFCore.Mask;
using Services;
using JsonMasking;

namespace DigitalBanking.ServiceExtensions
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private Span _span;
        private static readonly Counter errorsTotal = MetricsHelper.CreateCounter("errors_total", "Total number of ERRORS in route", new string[] { "route" });
        private static readonly Histogram http_request_duration_seconds = MetricsHelper.CreateHistogram("http_request_duration_seconds", "Duration of HTTP requests in seconds", new HistogramConfiguration
        {
            Buckets = new double[] { 0.1, 0.2, 0.3, 0.5, 0.7, 0.8, 0.9, 1, 3, 5, 7, 9 },
            LabelNames = new string[] { "method", "route", "code" }
        });
        private Stopwatch timer = null;
        private IMemoryCache memoryCache;
        private IConfiguration configuration;

        public RequestResponseLoggingMiddleware(RequestDelegate next, Span span, IMemoryCache cache, IConfiguration config)
        {
            _next = next;
            _span = span;
            timer = Stopwatch.StartNew();
            memoryCache = cache;
            configuration = config;
        }

        public async Task Invoke(HttpContext context, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            var maskService = context.RequestServices.GetRequiredService<Mask>();
            // Read and log request body data
            string requestBodyPayload = await ReadRequestBody(context.Request);
            logger.LogInformation($"Http Method: [{context.Request.Method}], Protoclol [{context.Request.Protocol}], Path [{context.Request.Path}], Request Body: [{requestBodyPayload}]");
            logger.LogInformation($"Http Headers: [{JsonSerializer.Serialize(context.Request.Headers)}]");
            logger.LogInformation($"Http Connection: [{context.Connection.RemoteIpAddress}:{context.Connection.RemotePort}]");
            logger.LogInformation($"Http Scheme: [{context.Request.Scheme}]");
            logger.LogInformation($"Http Host: [{context.Request.Host}]");

            //Mask maskProfile = maskService;
            var blacklist = new[] { "card" };

            // Read and log response body data
            // Copy a pointer to the original response body stream
            var originalResponseBodyStream = context.Response.Body;

            // Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                // ...and use that for the temporary response body
                context.Response.Body = responseBody;

                // Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                var responseBodyPayload = await ReadResponseBody(context.Response);

                if (context.Response.StatusCode != StatusCodes.Status200OK)
                {
                    errorsTotal.WithLabels(context.Request.Path.ToString()).Inc();
                }

                _span.getCurrentSpan().SetAttribute("response", responseBodyPayload);
                try
                {
                    var maskedResponse = responseBodyPayload.MaskFields(blacklist, "****");
                    logger.LogInformation($"Response Body: {maskedResponse}");
                }
                catch (Exception e) { }
                await responseBody.CopyToAsync(originalResponseBodyStream);
                timer.Stop();
                http_request_duration_seconds.SetLabelsWithElapsedTime(timer.Elapsed.TotalSeconds, context.Request.Method, context.Request.Path, context.Response.StatusCode.ToString());


            }
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);
            return $"{responseBody}";
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            HttpRequestRewindExtensions.EnableBuffering(request);

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            string requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{requestBody}";
        }
    }
}
