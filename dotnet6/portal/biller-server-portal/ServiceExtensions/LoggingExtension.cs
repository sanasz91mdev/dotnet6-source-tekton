using Microsoft.Extensions.Primitives;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SpectreConsole;

namespace biller_server_portal.ServiceExtensions
{
    public static partial class LoggingExtension
    {
            public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
            {
            var logFilePath = builder.Configuration.GetValue<string>("Logging:filepath");
            var fileBytes = builder.Configuration.GetValue<string>("Logging:rollingIntervalConfig:fileBytes");
            long.TryParse(fileBytes, out var bytesvalue);

            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogEventLevel.Information)
                    .WriteTo.Async(a => a.File(logFilePath, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}", rollingInterval: RollingInterval.Day,rollOnFileSizeLimit: true,fileSizeLimitBytes: bytesvalue))
                    .WriteTo.SpectreConsole(
                        "{Timestamp:HH:mm:ss} [{Level:u4}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
                        minLevel: LogEventLevel.Information)
                    .CreateLogger();


                builder.Host.UseSerilog();

                return builder;
            }
        
    }


    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;



        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context, ILogger<RequestLogContextMiddleware> logger)
        {
            var dictionary = new Dictionary<string, object>
    {
         { "CorrelationId", context.GetCorrelationId() },
         { "MachineName", Environment.MachineName}
    };

            // ensures all entries are tagged with some values
            using (logger.BeginScope(dictionary))
            {
                logger.LogInformation("Logging delegate");
                // Call the next delegate/middleware in the pipeline
                return _next(context);
            }
        }

    }


    public static class ContextExtension
    {
        public static string GetCorrelationId(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out StringValues correlationId);
            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }
    }
}
